using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Data2Lua.Classes;

namespace Data2Lua.UI
{
    public partial class Manager : Form
    {
        private string _Folder;
        private MainWindow _Parent;
        private int Total = 0;
        private int Errors = 0;
        private List<Item> Items;
        private List<string> _Errors;

        public Manager(MainWindow _parent, string __Folder)
        {
            InitializeComponent();
            this._Folder = __Folder;
            this._Parent = _parent;
            Items = new List<Item>();
            _Errors = new List<string>();
        }

        private void log(string _msg)
        {
            txtConsole.Invoke((MethodInvoker)delegate
            {
                txtConsole.AppendText(_msg + '\n');
            });
        }

        private void error(Item _item, string msg)
        {
            string _error = string.Format("[Erro] {0}: {1}", _item.Id, msg);
            _Errors.Add(_error);
            log(_error);
            Errors++;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerAsync();
            btnConvert.Enabled = false;
        }

        private void Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._Parent.Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Read item name files
                List<string> itemNames = FileManager.GetLines(string.Format("{0}\\{1}", _Folder, "idnum2itemdisplaynametable.txt"), Encoding.Default);
                string numItemNames = FileManager.GetText(string.Format("{0}\\{1}", _Folder, "num2itemdisplaynametable.txt"), Encoding.Default);

                // Read item description files
                string itemDescs = FileManager.GetText(string.Format("{0}\\{1}", _Folder, "idnum2itemdesctable.txt"), Encoding.Default);
                string numItemDescs = FileManager.GetText(string.Format("{0}\\{1}", _Folder, "num2itemdesctable.txt"), Encoding.Default);

                // Read item resource files
                string itemRes = FileManager.GetText(string.Format("{0}\\{1}", _Folder, "idnum2itemresnametable.txt"), Encoding.GetEncoding("ks_c_5601-1987"));
                string numItemRes = FileManager.GetText(string.Format("{0}\\{1}", _Folder, "num2itemresnametable.txt"), Encoding.GetEncoding("ks_c_5601-1987"));

                // Read item slots files
                string itemSlots = FileManager.GetText(string.Format("{0}\\{1}", _Folder, "itemslotcounttable.txt"), Encoding.Default);

                // Get items total
                Total = itemNames.Count;

                // Log it.
                log("[Info] Encontrados " + Total + " itens");

                for (int i = 0; i < itemNames.Count; i++)
                {
                    // Save the last quantity of errors
                    int _terrors = Errors;

                    // Stops on item #1000
                    // NOTE: debug only
                    if (i == 101) break;

                    // Get the block of the item
                    string _itemBlock = itemNames[i];

                    if (string.IsNullOrEmpty(_itemBlock))
                        continue;

                    // Create a new item instance
                    Item _item = new Item();

                    // ID#NAME#
                    _item.Id = int.Parse(_itemBlock.Split('#')[0]);
                    _item.IdentifiedDisplayName = _itemBlock.Split('#')[1];

                    // TODO
                    _item.ClassNum = 0;

                    // Change the current file mask
                    string mask = '\n' + _item.Id + "#";
                    if (numItemNames.Contains(mask))
                        _item.UnidentifiedDisplayName = numItemNames.Split(new string[] { mask }, StringSplitOptions.None)[1].Split(new char[] { '#' })[0];
                    else
                    {
                        // The unidentified display name has not found, so use the identified display name
                        _item.UnidentifiedDisplayName = _item.IdentifiedDisplayName;
                        //error(_item, "Nome de item não identificado não encontrado");
                    }

                    // desc
                    mask = '\n' + _item.Id + "#" + "\r\n";
                    if (itemDescs.Contains(mask))
                        _item.IdentifiedDescriptionName = itemDescs.Split(new string[] { mask }, StringSplitOptions.None)[1].Split(new string[] { "\n#" }, StringSplitOptions.None)[0];
                    else
                        error(_item, "Descrição não encontrada");

                    if (numItemDescs.Contains(mask))
                        _item.UnidentifiedDescriptionName = numItemDescs.Split(new string[] { mask }, StringSplitOptions.None)[1].Split(new string[] { "\n#" }, StringSplitOptions.None)[0];

                    // Check if descriptions equals
                    if (_item.IdentifiedDescriptionName.StartsWith(_item.UnidentifiedDescriptionName))
                        _item.UnidentifiedDescriptionName = ""; // save some diskspace

                    // resname
                    mask = '\n' + _item.Id + "#";
                    if (itemRes.Contains(mask))
                        _item.IdentifiedResourceName = itemRes.Split(new string[] { mask }, StringSplitOptions.None)[1].Split('#')[0];
                    else
                    {
                        log(string.Format("[Erro] {0}: {1}", "Imagem não encontrada", _item.Id));
                        Errors++;
                    }
                    if (numItemRes.Contains(mask))
                        _item.UnidentifiedResourceName = numItemRes.Split(new string[] { mask }, StringSplitOptions.None)[1].Split('#')[0];
                    else
                        _item.UnidentifiedResourceName = _item.IdentifiedResourceName;

                    // slots
                    if (itemSlots.Contains(mask))
                        _item.SlotCount = int.Parse(itemSlots.Split(new string[] { mask }, StringSplitOptions.None)[1].Split('#')[0]);

                    if(_terrors == Errors)
                        Items.Add(_item);

                    lbStatus.Invoke((MethodInvoker)delegate
                    {
                        lbStatus.Text = i + " itens verificados";
                    });
                    log(string.Format("[Add] {0}: {1}", _item.Id, _item.IdentifiedDisplayName));
                    float pct = (i / (float)Total) *100f;
                    backgroundWorker.ReportProgress((int)pct);
                }
            } catch (Exception _e)
            {
                MessageBox.Show(_e.Message + '\n' + '\n' + _e.StackTrace, "Erro fatal!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (File.Exists("itemInfo.lua"))
                File.Delete("itemInfo.lua");

            using (StreamWriter Writer = new StreamWriter(
                new FileStream("itemInfo.lua", FileMode.CreateNew, FileAccess.ReadWrite),
                Encoding.UTF8
                ))
            {
                Writer.Write("tbl = {\n");

                for (int i = 0; i < Items.Count; i++)
                {
                    Item _Item = Items.ElementAt(i);
                    Writer.Write(_Item.ToString() + ((i + 1 < Items.Count) ? ',' : ' ') + '\n');
                }

                Writer.Write("}\n\n");
                Writer.Write(txtLuaFunction.Text);
                Writer.Close();
            }

            if (Errors == 0)
                MessageBox.Show("Seu itemInfo.lua está pronto para o uso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                MessageBox.Show("Seu itemInfo.lua está pronto para o uso!\nNo entanto, houveram alguns erros durante a conversão.", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                new ErrorList(this._Errors).ShowDialog();
                lbErrors.Text = "Conversão finalizada com " + Errors + " erros";
            }

            Items.Clear();
            _Errors.Clear();
            Errors = 0;
            Total = 0;
            btnConvert.Enabled = true;

        }
    }
}
