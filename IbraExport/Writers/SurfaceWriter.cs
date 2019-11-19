using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace IbraExport.Writers
{
    class SurfaceWriter : Writer
    {
        public override bool TryDump(RhinoObject obj, Model model, RhinoDoc document)
        {
            if (obj.ObjectType != ObjectType.Surface)
                return false;

            var surface = (Surface)obj.Geometry;

            var surfaceKey = obj.GetKey();

            var geometryItem = new Item($"{surfaceKey}.NurbsSurfaceGeometry3D", "NurbsSurfaceGeometry3D");
            DumpNurbsSurface3D(geometryItem, surface.ToNurbsSurface(document.ModelAbsoluteTolerance, out var _));

            var surfaceItem = new Item(surfaceKey, "Surface3D");
            surfaceItem.Set("Geometry", geometryItem.Key);
            surfaceItem.Set("DomainU", surface.Domain(0));
            surfaceItem.Set("DomainV", surface.Domain(1));

            model.Items.Add(geometryItem);
            model.Items.Add(surfaceItem);

            return true;
        }
    }
}
