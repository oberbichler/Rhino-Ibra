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

            var brep = surface.ToBrep();

            if (!brep.MakeValidForV2())
                throw new Exception("Converting Surface to NURBS failed");

            var surfaceKey = obj.GetKey();

            var geometryItem = new Item($"{surfaceKey}.NurbsSurfaceGeometry3D", "NurbsSurfaceGeometry3D");
            DumpNurbsSurface3D(geometryItem, (NurbsSurface)brep.Surfaces[0]);

            var surfaceItem = new Item(surfaceKey, "Surface3D");
            surfaceItem.Set("geometry", geometryItem.Key);
            surfaceItem.Set("domain_u", surface.Domain(0));
            surfaceItem.Set("domain_v", surface.Domain(1));

            model.Items.Add(geometryItem);
            model.Items.Add(surfaceItem);

            return true;
        }
    }
}
