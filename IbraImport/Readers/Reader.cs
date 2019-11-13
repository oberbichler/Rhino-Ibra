using Newtonsoft.Json.Linq;
using Rhino;
using Rhino.DocObjects;
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

        public abstract bool TryLoad(JObject data, Model model, RhinoDoc document);
    }
}
