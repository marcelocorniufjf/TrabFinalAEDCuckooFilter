namespace CuckooFilterWindowsForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnLookup;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtValue;

        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            btnInsert = new Button();
            btnLookup = new Button();
            btnDelete = new Button();
            txtValue = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(14, 14);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(983, 173);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnInsert
            // 
            btnInsert.Location = new Point(14, 194);
            btnInsert.Margin = new Padding(4, 3, 4, 3);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new Size(88, 27);
            btnInsert.TabIndex = 1;
            btnInsert.Text = "&Inserir";
            btnInsert.UseVisualStyleBackColor = true;
            btnInsert.Click += btnInsert_Click;
            // 
            // btnLookup
            // 
            btnLookup.Location = new Point(108, 194);
            btnLookup.Margin = new Padding(4, 3, 4, 3);
            btnLookup.Name = "btnLookup";
            btnLookup.Size = new Size(88, 27);
            btnLookup.TabIndex = 2;
            btnLookup.Text = "&Localiizar";
            btnLookup.UseVisualStyleBackColor = true;
            btnLookup.Click += btnLookup_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(203, 194);
            btnDelete.Margin = new Padding(4, 3, 4, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(88, 27);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "&Excluir";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // txtValue
            // 
            txtValue.Location = new Point(298, 196);
            txtValue.Margin = new Padding(4, 3, 4, 3);
            txtValue.Name = "txtValue";
            txtValue.Size = new Size(116, 23);
            txtValue.TabIndex = 0;
            txtValue.Text = "1";
            txtValue.KeyDown += txtValue_KeyDown;
            txtValue.KeyPress += txtValue_KeyPress;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 231);
            Controls.Add(txtValue);
            Controls.Add(btnDelete);
            Controls.Add(btnLookup);
            Controls.Add(btnInsert);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            KeyPreview = true;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Cuckoo Filter - AED (Marcelo Corni Alves)";
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }

}
