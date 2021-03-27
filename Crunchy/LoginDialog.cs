using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crunchy
{
    public partial class LoginDialog : Form
    {
        public string Username => textBox1.Text;
        public string Password => textBox2.Text;

        public LoginDialog()
        {
            InitializeComponent();
        }

        private void LoginDialog_Load(object sender, EventArgs e)
        {
        }
    }
}