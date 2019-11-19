using Rhino;
using Rhino.DocObjects;
using Rhino.FileIO;
using Rhino.PlugIns;
using Rhino.UI;
using System;
using System.IO;

namespace IbraExport
{
    public class Info : FileExportPlugIn
    {
        public Info()
        {
            Instance = this;
        }

        public static Info Instance { get; private set; }

        protected override FileTypeList AddFileTypes(FileWriteOptions options)
        {
            var result = new FileTypeList();
            result.AddFileType("IBRA-Model (*.ibra)", "ibra", true);
            return result;
        }

        protected override void DisplayOptionsDialog(IntPtr parent, string description, string extension)
        {
            var dialog = new ExportSettingsDialog();
            dialog.ShowDialog();
        }

        protected override WriteFileResult WriteFile(string filename, int index, RhinoDoc doc, FileWriteOptions options)
        {
            try
            {
                var filter = new ObjectEnumeratorSettings();

                if (options.WriteSelectedObjectsOnly)
                    filter.SelectedObjectsFilter = options.WriteSelectedObjectsOnly;

                var objs = doc.Objects.FindByFilter(filter);

                var model = new Model();
                model.Add(objs);

                using (var writer = new StreamWriter(filename))
                    model.Save(writer);

                return WriteFileResult.Success;
            }
            catch (Exception ex) when (!System.Diagnostics.Debugger.IsAttached)
            {
                Dialogs.ShowMessage(ex.Message, "Error", ShowMessageButton.OK, ShowMessageIcon.Error);

                return WriteFileResult.Failure;
            }
        }
    }
}
