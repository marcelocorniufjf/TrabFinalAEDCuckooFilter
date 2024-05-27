using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CuckooFilterWindowsForms
{
    public class CuckooFilter
    {
        private int bucketCount;
        private readonly int bucketSize;
        private readonly int fingerprintSize;
        private readonly int maxKicks;
        private byte[][] table;
        private readonly List<int> insertedKeys;
        private readonly PictureBox pictureBox;
        private string operationDetails;

        public CuckooFilter(int bucketCount, int bucketSize, int fingerprintSize, int maxKicks, PictureBox pictureBox)
        {
            this.bucketCount = bucketCount;
            this.bucketSize = bucketSize;
            this.fingerprintSize = fingerprintSize;
            this.maxKicks = maxKicks;
            this.table = new byte[bucketCount][];
            for (int i = 0; i < bucketCount; i++)
            {
                this.table[i] = new byte[bucketSize];
            }
            this.insertedKeys = new List<int>();
            this.pictureBox = pictureBox;
            this.operationDetails = "";
            DrawFilter();
        }

        private int Hash1(int key)
        {
            return Math.Abs((key ^ 0x5bd1e995) * 0x5bd1e995) % bucketCount;
        }

        private int Hash2(byte fingerprint, int hash1)
        {
            return (hash1 ^ fingerprint) % bucketCount;
        }

        private byte[] GetFingerprint(int key)
        {
            var hash = Math.Abs((key ^ 0x5bd1e995) * 0x5bd1e995);
            return BitConverter.GetBytes(hash).Take(fingerprintSize).ToArray();
        }

        public bool Insert(int key)
        {
            if (insertedKeys.Contains(key))
            {
                operationDetails = $"Valor duplicado {key} não inserido.";
                DrawFilter();
                return false;
            }

            byte[] fingerprint = GetFingerprint(key);
            int hash1 = Hash1(key);
            int hash2 = Hash2(fingerprint[0], hash1);

            bool inserted = InsertIntoBucket(hash1, fingerprint) || InsertIntoBucket(hash2, fingerprint);
            if (inserted)
            {
                insertedKeys.Add(key);
                operationDetails = $"Inserido {key} com hash1: {hash1}, hash2: {hash2}, fingerprint: {fingerprint[0]}";
                DrawFilter(new[] { hash1, hash2 });
                return true;
            }

            int currentHash = hash1;
            byte[] currentFingerprint = fingerprint;
            for (int i = 0; i < maxKicks; i++)
            {
                int index = new Random().Next(bucketSize);
                byte temp = table[currentHash][index];
                table[currentHash][index] = currentFingerprint[0];
                currentFingerprint[0] = temp;

                currentHash = Hash2(currentFingerprint[0], currentHash);
                if (InsertIntoBucket(currentHash, currentFingerprint))
                {
                    insertedKeys.Add(key);
                    operationDetails = $"Inserido após kick {key} com hash1: {hash1}, hash2: {hash2}, fingerprint: {fingerprint[0]}";
                    DrawFilter(new[] { hash1, hash2 });
                    return true;
                }
            }

            Rehash();
            return Insert(key);
        }

        private bool InsertIntoBucket(int bucket, byte[] fingerprint)
        {
            for (int i = 0; i < bucketSize; i++)
            {
                if (table[bucket][i] == 0)
                {
                    table[bucket][i] = fingerprint[0];
                    return true;
                }
            }
            return false;
        }

        public bool Lookup(int key)
        {
            byte[] fingerprint = GetFingerprint(key);
            int hash1 = Hash1(key);
            int hash2 = Hash2(fingerprint[0], hash1);

            bool found = BucketContains(hash1, fingerprint) || BucketContains(hash2, fingerprint);
            if (found)
            {
                operationDetails = $"Encontrado {key} com hash1: {hash1}, hash2: {hash2}, fingerprint: {fingerprint[0]}";
                DrawFilter(new[] { hash1, hash2 }, fingerprint[0], Color.Green);
                return true;
            }

            operationDetails = $"Não encontrado {key} com hash1: {hash1}, hash2: {hash2}, fingerprint: {fingerprint[0]}";
            DrawFilter(new[] { hash1, hash2 }, fingerprint[0], Color.Red);
            return false;
        }

        public bool Delete(int key)
        {
            byte[] fingerprint = GetFingerprint(key);
            int hash1 = Hash1(key);
            int hash2 = Hash2(fingerprint[0], hash1);

            if (DeleteFromBucket(hash1, fingerprint) || DeleteFromBucket(hash2, fingerprint))
            {
                insertedKeys.Remove(key);
                operationDetails = $"Deletado {key} com hash1: {hash1}, hash2: {hash2}, fingerprint: {fingerprint[0]}";
                DrawFilter();
                return true;
            }

            operationDetails = $"Não encontrado para deletar {key} com hash1: {hash1}, hash2: {hash2}, fingerprint: {fingerprint[0]}";
            DrawFilter();
            return false;
        }

        private bool DeleteFromBucket(int bucket, byte[] fingerprint)
        {
            for (int i = 0; i < bucketSize; i++)
            {
                if (table[bucket][i] == fingerprint[0])
                {
                    table[bucket][i] = 0;
                    return true;
                }
            }
            return false;
        }

        private bool BucketContains(int bucket, byte[] fingerprint)
        {
            for (int i = 0; i < bucketSize; i++)
            {
                if (table[bucket][i] == fingerprint[0])
                {
                    return true;
                }
            }
            return false;
        }

        private void Rehash()
        {
            var oldTable = table;
            var oldInsertedKeys = new List<int>(insertedKeys);
            insertedKeys.Clear();
            bucketCount *= 2;
            table = new byte[bucketCount][];
            for (int i = 0; i < bucketCount; i++)
            {
                table[i] = new byte[bucketSize];
            }

            foreach (var key in oldInsertedKeys)
            {
                Insert(key);
            }

            operationDetails = "Rehashing completo";
            DrawFilter();
        }

        private void DrawFilter(int[] highlightedBuckets = null, byte highlightedFingerprint = 0, Color? highlightColor = null)
        {
            Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                for (int i = 0; i < bucketCount; i++)
                {
                    bool isHighlighted = highlightedBuckets != null && highlightedBuckets.Contains(i);
                    Color color = isHighlighted ? highlightColor ?? Color.Blue : (table[i].Any(f => f != 0) ? Color.Blue : Color.White);
                    DrawBucket(g, i, color, highlightedFingerprint, isHighlighted ? Color.Black : (Color?)null);
                }

                DrawFilterParameters(g);
                DrawOperationDetails(g);
                DrawInsertedKeys(g);
            }
            pictureBox.Image = bitmap;
        }

        private void DrawBucket(Graphics g, int index, Color color, byte highlightedFingerprint, Color? highlightColor)
        {
            int bucketWidth = pictureBox.Width / bucketCount;
            int x = index * bucketWidth;
            g.FillRectangle(new SolidBrush(color), x, 20, bucketWidth, pictureBox.Height / 2);
            g.DrawRectangle(Pens.Black, x, 20, bucketWidth, pictureBox.Height / 2);

            for (int i = 0; i < bucketSize; i++)
            {
                string fingerprint = table[index][i] == 0 ? " " : table[index][i].ToString();
                Font font = SystemFonts.DefaultFont;
                if (table[index][i] == highlightedFingerprint && highlightColor.HasValue)
                {
                    font = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
                }
                g.DrawString(fingerprint, font, Brushes.Black, x + 5, 25 + (i * 20));
            }

            g.DrawString(index.ToString(), SystemFonts.DefaultFont, Brushes.Black, x, 0);
        }

        private void DrawFilterParameters(Graphics g)
        {
            string parameters = $"Buckets: {bucketCount}, Bucket Size: {bucketSize}, Fingerprint Size: {fingerprintSize} bits, Max Kicks: {maxKicks}";
            g.DrawString(parameters, SystemFonts.DefaultFont, Brushes.Black, 10, pictureBox.Height - 60);
        }

        private void DrawOperationDetails(Graphics g)
        {
            g.DrawString(operationDetails, SystemFonts.DefaultFont, Brushes.Black, 10, pictureBox.Height - 40);
        }

        private void DrawInsertedKeys(Graphics g)
        {
            string values = $"Chaves inseridas: {string.Join(", ", insertedKeys)}";
            g.DrawString(values, SystemFonts.DefaultFont, Brushes.Magenta, 10, pictureBox.Height - 20);
        }

        public void HighlightBuckets(int hash1, int hash2, Color color)
        {
            DrawFilter(new[] { hash1, hash2 }, highlightColor: color);
        }
    }

}
