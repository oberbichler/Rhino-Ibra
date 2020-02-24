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
    public abstract class Reader
    {
        public abstract string Name { get; }

        protected virtual bool IsActiveByDefault => true;

        public bool IsActive
        {
            get
            {
                return Info.Instance.Settings.GetBool($"Enable{Name}", IsActiveByDefault);
            }
            set
            {
                Info.Instance.Settings.SetBool($"Enable{Name}", value);
            }
        }

        public static ObjectAttributes GetAttributes(RhinoDoc document, JObject data)
        {
            var attributes = document.CreateDefaultAttributes();

            if (data.TryGetValue<string>("key", out var key))
                attributes.Name = key;

            if (data.TryGetValue<string>("color", out var color))
            {
                var argb = int.Parse(color.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);

                attributes.ColorSource = ObjectColorSource.ColorFromObject;
                attributes.ObjectColor = System.Drawing.Color.FromArgb(argb);
            }

            if (data.TryGetValue<string>("layer", out var layer))
            {
                var layerIndex = document.Layers.FindByFullPath(layer, RhinoMath.UnsetIntIndex);

                if (layerIndex == RhinoMath.UnsetIntIndex)
                    layerIndex = document.Layers.Add(layer, System.Drawing.Color.Black);

                attributes.LayerIndex = layerIndex;
            }

            if (data.TryGetValue<string>("arrowhead", out var arrowhead))
            {
                if (arrowhead == "start")
                    attributes.ObjectDecoration = ObjectDecoration.StartArrowhead;
                if (arrowhead == "end")
                    attributes.ObjectDecoration = ObjectDecoration.EndArrowhead;
                if (arrowhead == "both")
                    attributes.ObjectDecoration = ObjectDecoration.BothArrowhead;
            }

            return attributes;
        }

        protected NurbsCurve GetNurbsCurve2D(JObject data)
        {
            var degree = data["degree"].As<int>();
            var nbPoles = data["nb_poles"].As<int>();
            var knots = data["knots"].AsList<double>();
            var poles = data["poles"].AsListOfPoint2d();
            var isRational = data.ContainsKey("weights");

            var nurbs = new NurbsCurve(3, isRational, degree + 1, nbPoles);

            for (int i = 0; i < knots.Count; i++)
                nurbs.Knots[i] = knots[i];

            if (isRational)
            {
                var weights = data["weights"].AsList<double>();

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
            var degree = data["degree"].As<int>();
            var nbPoles = data["nb_poles"].As<int>();
            var knots = data["knots"].AsList<double>();
            var poles = data["poles"].AsListOfPoint3d();
            var isRational = data.ContainsKey("weights");

            var nurbs = new NurbsCurve(3, isRational, degree + 1, nbPoles);

            for (int i = 0; i < knots.Count; i++)
                nurbs.Knots[i] = knots[i];

            if (isRational)
            {
                var weights = data["weights"].AsList<double>();

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
            var degreeU = data["degree_u"].As<int>();
            var degreeV = data["degree_v"].As<int>();
            var nbPolesU = data["nb_poles_u"].As<int>();
            var nbPolesV = data["nb_poles_v"].As<int>();
            var knotsU = data["knots_u"].AsList<double>();
            var knotsV = data["knots_v"].AsList<double>();
            var poles = data["poles"].AsListOfPoint2d();
            var isRational = data.ContainsKey("weights");

            var nurbs = NurbsSurface.Create(3, isRational, degreeU + 1, degreeV + 1, nbPolesU, nbPolesV);

            for (int i = 0; i < knotsU.Count; i++)
                nurbs.KnotsU[i] = knotsU[i];

            for (int i = 0; i < knotsV.Count; i++)
                nurbs.KnotsV[i] = knotsV[i];

            if (isRational)
            {
                var weights = data["weights"].AsList<double>();

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
            var degreeU = data["degree_u"].As<int>();
            var degreeV = data["degree_v"].As<int>();
            var nbPolesU = data["nb_poles_u"].As<int>();
            var nbPolesV = data["nb_poles_v"].As<int>();
            var knotsU = data["knots_u"].AsList<double>();
            var knotsV = data["knots_v"].AsList<double>();
            var poles = data["poles"].AsListOfPoint3d();
            var isRational = data.ContainsKey("weights");

            var nurbs = NurbsSurface.Create(3, isRational, degreeU + 1, degreeV + 1, nbPolesU, nbPolesV);

            for (int i = 0; i < knotsU.Count; i++)
                nurbs.KnotsU[i] = knotsU[i];

            for (int i = 0; i < knotsV.Count; i++)
                nurbs.KnotsV[i] = knotsV[i];

            if (isRational)
            {
                var weights = data["weights"].AsList<double>();

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
