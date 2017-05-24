using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Data2Lua.UI
{
    public partial class ErrorList : Form
    {
        private List<string> Errors;

        public ErrorList(List<string> _errors)
        {
            InitializeComponent();

            Errors = _errors;
        }

        private void ErrorList_Load(object sender, EventArgs e)
        {
            foreach(string _error in Errors)
                txtErrors.AppendText(_error + '\n');
        }
    }
}
