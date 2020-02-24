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

            var geometryItem = new Item($"{surfaceKey}.nurbs_surface_geometry_3d", "nurbs_surface_geometry_3d");
            DumpNurbsSurface3D(geometryItem, surface.ToNurbsSurface(document.ModelAbsoluteTolerance, out var _));

            var surfaceItem = new Item(surfaceKey, "surface_3d");
            surfaceItem.Set("geometry", geometryItem.Key);
            surfaceItem.Set("domain_u", surface.Domain(0));
            surfaceItem.Set("domain_v", surface.Domain(1));

            model.Items.Add(geometryItem);
            model.Items.Add(surfaceItem);

            return true;
        }
    }
}
