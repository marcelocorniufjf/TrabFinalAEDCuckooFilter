using System.Windows.Forms;

namespace CuckooFilterWindowsForms
{
    public partial class Form1 : Form
    {
        private CuckooFilter cuckooFilter;

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
        }

        private void btnLookup_Click(object sender, EventArgs e)
        {
            int value = int.Parse(txtValue.Text);
            cuckooFilter.Lookup(value);
            txtValue.Focus();
            txtValue.SelectAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int value = int.Parse(txtValue.Text);
            cuckooFilter.Delete(value);
            txtValue.Focus();
            txtValue.SelectAll();
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
    }
}
