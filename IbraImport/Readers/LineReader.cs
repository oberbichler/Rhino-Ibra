﻿using Newtonsoft.Json.Linq;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbraImport.Readers
{
    class LineReader : Reader
    {
        public override string Name => "LineReader";

        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType(out int index, "Line2D", "Line3D"))
                return false;

            var attributes = GetAttributes(document, data);

            Line line;

            if (index == 0) // 2D
            {
                var a = data["a"].AsPoint2d();
                var b = data["b"].AsPoint2d();

                line = new Line(a.ToPoint3d(), b.ToPoint3d());
            }
            else            // 3D
            {
                var a = data["a"].AsPoint3d();
                var b = data["b"].AsPoint3d();

                line = new Line(a, b);
            }

            document.Objects.AddLine(line, attributes);

            return true;
        }
    }
}
