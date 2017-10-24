using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace StadiaCrowdAnalytics_Alligator
{
    public class Gradient
    {
        private double mdMaxGradient;
        private double mdMinGradient;
        protected Grasshopper.GUI.Gradient.GH_Gradient mgGradient;

        Grasshopper.Kernel.GH_ActiveObject mOwner;

        public Gradient(Grasshopper.Kernel.GH_ActiveObject owner)
        {
            mOwner = owner;

            //Gradient set up
            mdMaxGradient = 1;
            mdMinGradient = 0;
            mgGradient = DefaultGradient;
        }

        public double MaxGradient
        {
            get { return mdMaxGradient; }
            set { mdMaxGradient = value; }
        }

        public double MinGradient
        {
            get { return mdMinGradient; }
            set { mdMinGradient = value; }
        }

        public Grasshopper.GUI.Gradient.GH_Gradient SWFGradient
        {
            get
            {
                return this.mgGradient;
            }

            set
            {
                if (value != null)
                {
                    if (this.mgGradient != null)
                    {
                        this.mgGradient.GradientChanged -= new Grasshopper.GUI.Gradient.GH_Gradient.GradientChangedEventHandler(this.GradientChanged);
                        this.mgGradient.SelectionChanged -= new Grasshopper.GUI.Gradient.GH_Gradient.SelectionChangedEventHandler(this.SelectionChanged);
                        this.mgGradient = null;
                    }
                    if (value != null)
                    {
                        this.mgGradient = value;
                        this.mgGradient.GradientChanged += new Grasshopper.GUI.Gradient.GH_Gradient.GradientChangedEventHandler(this.GradientChanged);
                        this.mgGradient.SelectionChanged += new Grasshopper.GUI.Gradient.GH_Gradient.SelectionChangedEventHandler(this.SelectionChanged);
                    }
                }
            }
        }

        public static Grasshopper.GUI.Gradient.GH_Gradient DefaultGradient
        {
            get
            {
                Grasshopper.Kernel.GH_SettingsServer server = new Grasshopper.Kernel.GH_SettingsServer("grasshopper_gradientsSSA");
                if (server.Count == 0)
                {
                    return Grasshopper.GUI.Gradient.GH_Gradient.Traffic();
                }
                if (!server.ConstainsEntry("default"))
                {
                    return Grasshopper.GUI.Gradient.GH_Gradient.Traffic();
                }
                string str = server.GetValue("default", string.Empty);
                if (string.IsNullOrEmpty(str))
                {
                    return Grasshopper.GUI.Gradient.GH_Gradient.Traffic();
                }
                Grasshopper.GUI.Gradient.GH_Gradient gradient2 = Xml2Gradient(str);
                if (gradient2 == null)
                {
                    return Grasshopper.GUI.Gradient.GH_Gradient.Traffic();
                }
                return gradient2;
            }
            set
            {
                if (value == null)
                {
                    value = new Grasshopper.GUI.Gradient.GH_Gradient();
                    value.AddGrip(0.5, Color.White);
                }
                Grasshopper.Kernel.GH_SettingsServer server = new Grasshopper.Kernel.GH_SettingsServer("grasshopper_gradientsSSA");
                server.SetValue("default", Gradient2Xml(value));
                server.WritePersistentSettings();
            }
        }

        private static Grasshopper.GUI.Gradient.GH_Gradient Xml2Gradient(string xml)
        {
            GH_IO.Serialization.GH_LooseChunk reader = new GH_IO.Serialization.GH_LooseChunk("gradient");
            reader.Deserialize_Xml(xml);
            Grasshopper.GUI.Gradient.GH_Gradient gradient = new Grasshopper.GUI.Gradient.GH_Gradient();
            if (!gradient.Read(reader))
            {
                return null;
            }
            return gradient;
        }

        private static string Gradient2Xml(Grasshopper.GUI.Gradient.GH_Gradient grad)
        {
            GH_IO.Serialization.GH_LooseChunk writer = new GH_IO.Serialization.GH_LooseChunk("gradient");
            if (!grad.Write(writer))
            {
                return null;
            }
            return writer.Serialize_Xml();
        }

        private void GradientChanged(object sender, Grasshopper.GUI.Gradient.GH_GradientChangedEventArgs e)
        {
            mOwner.ExpireSolution(true);
        }

        private void SelectionChanged(object sender, Grasshopper.GUI.Gradient.GH_GradientChangedEventArgs e)
        {
            Grasshopper.Instances.RedrawCanvas();
        }

    }
}
