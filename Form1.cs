using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CuckooFilterWindowsForms
{
    public partial class Form1 : Form
    {
        private CuckooFilter cuckooFilter;
        private string operacao = "";

        public Form1()
        {
            InitializeComponent();
            cuckooFilter = new CuckooFilter(16, 4, 4, 500, pictureBox1);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int value = int.Parse(txtValue.Text);
            cuckooFilter.Insert(value);
            txtValue.Focus();
            txtValue.SelectAll();
            operacao = $"inserir_{txtValue.Text}";
        }

        private void btnLookup_Click(object sender, EventArgs e)
        {
            int value = int.Parse(txtValue.Text);
            cuckooFilter.Lookup(value);
            txtValue.Focus();
            txtValue.SelectAll();
            operacao = $"buscar_{txtValue.Text}";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int value = int.Parse(txtValue.Text);
            cuckooFilter.Delete(value);
            txtValue.Focus();
            txtValue.SelectAll();
            operacao = $"excluir_{txtValue.Text}";
        }

        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnInsert_Click(sender, e);
            }
        }

        private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt == true && e.KeyCode == Keys.P)
            {
                pictureBox1.Image.Save($"E:\\Mestrado\\Disciplinas\\AED\\Trabalho Final\\Imagens\\CF_{operacao}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg", ImageFormat.Png);
            }
        }
    }
}
