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
    class CurveWriter : Writer
    {
        public override bool TryDump(RhinoObject obj, Model model, RhinoDoc document)
        {
            if (obj.ObjectType != ObjectType.Curve)
                return false;

            var curve = (Curve)obj.Geometry;

            var key = obj.GetKey();

            var geometryItem = new Item($"{key}.NurbsCurveGeometry3D", "NurbsCurveGeometry3D");
            DumpNurbsCurve3D(geometryItem, curve.ToNurbsCurve());
            
            var curveItem = new Item(key, "Curve3D");
            curveItem.Set("Geometry", geometryItem.Key);
            curveItem.Set("Domain", curve.Domain);

            model.Items.Add(geometryItem);
            model.Items.Add(curveItem);

            return true;
        }
    }
}
