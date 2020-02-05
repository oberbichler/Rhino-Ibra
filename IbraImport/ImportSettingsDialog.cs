using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IbraImport
{
    public partial class ImportSettingsDialog : Form
    {
        public ImportSettingsDialog()
        {
            InitializeComponent();
        }

        struct ReaderItem
        {
            public ReaderItem(Readers.Reader reader)
            {
                Reader = reader;
            }

            public Readers.Reader Reader { get; }

            public override string ToString()
            {
                return Reader.Name;
            }
        }

        private void ImportSettingsDialog_Load(object sender, EventArgs e)
        {
            foreach (var reader in Model.Readers)
                readersCheckedListBox.Items.Add(new ReaderItem(reader), reader.IsActive);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < readersCheckedListBox.Items.Count; i++)
            {
                var reader = ((ReaderItem)readersCheckedListBox.Items[i]).Reader;
                reader.IsActive = readersCheckedListBox.GetItemChecked(i);
            }
        }
    }
}
