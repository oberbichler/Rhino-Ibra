using Rhino;
using Rhino.FileIO;
using Rhino.PlugIns;
using Rhino.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AModelImport
{
    public class Info : FileImportPlugIn
    {
        public Info()
        {
            Instance = this;
        }

        public static Info Instance { get; private set; }

        protected override FileTypeList AddFileTypes(FileReadOptions options)
        {
            var result = new FileTypeList();
            result.AddFileType("AModel (*.amodel)", "amodel");
            return result;
        }

        protected override bool ReadFile(string filename, int index, RhinoDoc document, FileReadOptions options)
        {
            try
            {
                var model = default(Model);

                using (var reader = new StreamReader(filename))
                {
                    model = Model.Load(reader);
                }

                model.LoadItems(document);
            }
            catch (Exception ex) when (!System.Diagnostics.Debugger.IsAttached)
            {
                Dialogs.ShowMessage(ex.Message, "Error", ShowMessageButton.OK, ShowMessageIcon.Error);

                return false;
            }

            return true;
        }
    }
}
