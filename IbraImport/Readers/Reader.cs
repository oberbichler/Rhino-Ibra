using Newtonsoft.Json.Linq;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbraImport.Readers
{
    abstract class Reader
    {
        public static ObjectAttributes GetAttributes(RhinoDoc document, JObject data)
        {
            var attributes = document.CreateDefaultAttributes();

            if (data.TryGetValue<string>("Key", out var key))
                attributes.Name = key;

            if (data.TryGetValue<string>("Color", out var color))
            {
                var argb = int.Parse(color.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);

                attributes.ColorSource = ObjectColorSource.ColorFromObject;
                attributes.ObjectColor = System.Drawing.Color.FromArgb(argb);
            }

            if (data.TryGetValue<string>("Layer", out var layer))
            {
                var layerIndex = document.Layers.FindByFullPath(layer, RhinoMath.UnsetIntIndex);

                if (layerIndex == RhinoMath.UnsetIntIndex)
                    layerIndex = document.Layers.Add(layer, System.Drawing.Color.Black);

                attributes.LayerIndex = layerIndex;
            }

            if (data.TryGetValue<string>("Arrowhead", out var arrowhead))
            {
                if (arrowhead == "Start")
                    attributes.ObjectDecoration = ObjectDecoration.StartArrowhead;
                if (arrowhead == "End")
                    attributes.ObjectDecoration = ObjectDecoration.EndArrowhead;
                if (arrowhead == "Both")
                    attributes.ObjectDecoration = ObjectDecoration.BothArrowhead;
            }

            return attributes;
        }

        protected NurbsCurve GetNurbsCurve2D(JObject data)
        {
            var degree = data["Degree"].As<int>();
            var nbPoles = data["NbPoles"].As<int>();
            var knots = data["Knots"].AsList<double>();
            var poles = data["Poles"].AsListOfPoint2d();
            var isRational = data.ContainsKey("Weights");

            var nurbs = new NurbsCurve(3, isRational, degree + 1, nbPoles);

            for (int i = 0; i < knots.Count; i++)
                nurbs.Knots[i] = knots[i];

            if (isRational)
            {
                var weights = data["Weights"].AsList<double>();

                for (int i = 0; i < weights.Count; i++)
                {
                    var weight = weights[i];
                    var pole = poles[i] * weight;

                    nurbs.Points.SetPoint(i, pole[0], pole[1], 0, weight);
                }
            }
            else
            {
                for (int i = 0; i < poles.Count; i++)
                {
                    var pole = poles[i];

                    nurbs.Points.SetPoint(i, pole[0], pole[1], 0);
                }
            }

            return nurbs;
        }

        protected NurbsCurve GetNurbsCurve3D(JObject data)
        {
            var degree = data["Degree"].As<int>();
            var nbPoles = data["NbPoles"].As<int>();
            var knots = data["Knots"].AsList<double>();
            var poles = data["Poles"].AsListOfPoint3d();
            var isRational = data.ContainsKey("Weights");

            var nurbs = new NurbsCurve(3, isRational, degree + 1, nbPoles);

            for (int i = 0; i < knots.Count; i++)
                nurbs.Knots[i] = knots[i];

            if (isRational)
            {
                var weights = data["Weights"].AsList<double>();

                for (int i = 0; i < weights.Count; i++)
                {
                    var weight = weights[i];
                    var pole = poles[i] * weight;

                    nurbs.Points.SetPoint(i, pole[0], pole[1], pole[2], weight);
                }
            }
            else
            {
                for (int i = 0; i < poles.Count; i++)
                {
                    var pole = poles[i];

                    nurbs.Points.SetPoint(i, pole[0], pole[1], pole[2]);
                }
            }

            return nurbs;
        }

        protected NurbsSurface GetNurbsSurface2D(JObject data)
        {
            var degreeU = data["DegreeU"].As<int>();
            var degreeV = data["DegreeV"].As<int>();
            var nbPolesU = data["NbPolesU"].As<int>();
            var nbPolesV = data["NbPolesV"].As<int>();
            var knotsU = data["KnotsU"].AsList<double>();
            var knotsV = data["KnotsV"].AsList<double>();
            var poles = data["Poles"].AsListOfPoint2d();
            var isRational = data.ContainsKey("Weights");

            var nurbs = NurbsSurface.Create(3, isRational, degreeU + 1, degreeV + 1, nbPolesU, nbPolesV);

            for (int i = 0; i < knotsU.Count; i++)
                nurbs.KnotsU[i] = knotsU[i];

            for (int i = 0; i < knotsV.Count; i++)
                nurbs.KnotsV[i] = knotsV[i];

            if (isRational)
            {
                var weights = data["Weights"].AsList<double>();

                for (int i = 0; i < weights.Count; i++)
                {
                    var u = i / nbPolesV;
                    var v = i % nbPolesV;

                    var weight = weights[i];
                    var pole = poles[i] * weight;

                    nurbs.Points.SetPoint(u, v, pole[0], pole[1], pole[2], weight);
                }
            }
            else
            {
                for (int i = 0; i < poles.Count; i++)
                {
                    var u = i / nbPolesV;
                    var v = i % nbPolesV;

                    var pole = poles[i];

                    nurbs.Points.SetPoint(u, v, pole[0], pole[1], 0);
                }
            }

            if (!nurbs.IsValidWithLog(out var log))
                throw new Exception($"Invalid NurbsSurface: {log}");

            return nurbs;
        }

        protected NurbsSurface GetNurbsSurface3D(JObject data)
        {
            var degreeU = data["DegreeU"].As<int>();
            var degreeV = data["DegreeV"].As<int>();
            var nbPolesU = data["NbPolesU"].As<int>();
            var nbPolesV = data["NbPolesV"].As<int>();
            var knotsU = data["KnotsU"].AsList<double>();
            var knotsV = data["KnotsV"].AsList<double>();
            var poles = data["Poles"].AsListOfPoint3d();
            var isRational = data.ContainsKey("Weights");

            var nurbs = NurbsSurface.Create(3, isRational, degreeU + 1, degreeV + 1, nbPolesU, nbPolesV);

            for (int i = 0; i < knotsU.Count; i++)
                nurbs.KnotsU[i] = knotsU[i];

            for (int i = 0; i < knotsV.Count; i++)
                nurbs.KnotsV[i] = knotsV[i];

            if (isRational)
            {
                var weights = data["Weights"].AsList<double>();

                for (int i = 0; i < weights.Count; i++)
                {
                    var u = i / nbPolesV;
                    var v = i % nbPolesV;

                    var weight = weights[i];
                    var pole = poles[i] * weight;

                    nurbs.Points.SetPoint(u, v, pole[0], pole[1], pole[2], weight);
                }
            }
            else
            {
                for (int i = 0; i < poles.Count; i++)
                {
                    var u = i / nbPolesV;
                    var v = i % nbPolesV;

                    var pole = poles[i];

                    nurbs.Points.SetPoint(u, v, pole[0], pole[1], pole[2]);
                }
            }

            if (!nurbs.IsValidWithLog(out var log))
                throw new Exception($"Invalid NurbsSurface: {log}");

            return nurbs;
        }

        public abstract bool TryLoad(JObject data, Model model, RhinoDoc document);
    }
}
