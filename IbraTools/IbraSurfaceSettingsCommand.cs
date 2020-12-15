using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using System;

namespace IbraTools
{
    public class IbraSurfaceSettingsCommand : Command
    {
        static IbraSurfaceSettingsCommand _instance;

        public IbraSurfaceSettingsCommand()
        {
            _instance = this;
        }

        public static IbraSurfaceSettingsCommand Instance => _instance;

        public override string EnglishName => "IbraSurfaceSettings";

        GeometryBase GetGeometry(GeometryBase geometry)
        {
            switch (geometry)
            {
                case BrepFace brepFace:
                    return brepFace.UnderlyingSurface();
            }

            return null;
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var result = RhinoGet.GetMultipleObjects("Select faces", false, Rhino.DocObjects.ObjectType.Surface, out var rhObjects);

            if (result != Result.Success)
                return result;

            var dialog = new ExportSettingsDialog();

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return Result.Cancel;

            foreach (var rhObject in rhObjects)
            {
                var geometry = GetGeometry(rhObject.Geometry());

                if (geometry == null)
                    continue;

                dialog.SaveData(geometry);
            }

            return Result.Success;
        }
    }
}