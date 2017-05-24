using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

using Data2Lua.UI;

namespace Data2Lua
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FolderBrowserDialog folderDialog;
        private List<string> fileList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Localização da sua pasta data.";
            folderDialog.RootFolder = Environment.SpecialFolder.Desktop;
            folderDialog.ShowNewFolderButton = false;

            fileList.Add("idnum2itemdisplaynametable.txt");
            fileList.Add("idnum2itemdesctable.txt");
            fileList.Add("idnum2itemresnametable.txt");
            fileList.Add("num2itemdisplaynametable.txt");
            fileList.Add("num2itemdesctable.txt");
            fileList.Add("num2itemresnametable.txt");
            fileList.Add("itemslotcounttable.txt");
            fileList.Add("itemslotcounttable.txt");
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            folderDialog.ShowDialog();

            if (!string.IsNullOrEmpty(folderDialog.SelectedPath))
                txtFolder.Text = folderDialog.SelectedPath;
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            foreach(string _File in fileList)
            {
                string _Folder = txtFolder.Text;
                string _AbsolutePath = string.Format("{0}\\{1}", _Folder, _File);

                if (!File.Exists(_AbsolutePath))
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Não foi possível encontrar o arquivo {0}.", _File), "Houve um problema!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            this.Hide();
            new Manager(this, txtFolder.Text).Show();
        }
    }
}
