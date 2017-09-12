using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;
using BHB = BH.oM.Base;
using D3T = D3_Toolkit;

namespace Alligator.D3
{
    public class BubbleChart : GH_Component
    {
        public BubbleChart() : base("BubbleChart", "BubbleChart", "Create a bubble chart", "Alligator", "D3")
        {
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return D3_Alligator.Properties.Resources.BH.oM_D3_BubbleChart; }
        }

        public override void AddedToDocument(GH_Document document)
        {
            chart = new D3T.BubbleChart();
            chart.BrushEvent += OnBrush;
            newResult = false;
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("3299F9F5-7687-4D4B-B970-1F619E1297CA");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("data", "data", "list of objects", GH_ParamAccess.list);
            pManager.AddGenericParameter("axes", "axe", "list of property names used for axes", GH_ParamAccess.list);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("data", "data", "list of filtered objects", GH_ParamAccess.list);
            pManager.AddNumberParameter("indices", "indices", "list of indices from original list", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (newResult)
            {
                newResult = false;
                DA.SetDataList(0, result.ToList());
                DA.SetDataList(1, indices.ToList());
            }
            else
            {
                List<string> axes = new List<string>();
                if (DA.GetDataList<string>(1, axes))
                    chart.setAxes(axes);

                List<object> data = new List<object>();
                DA.GetDataList<object>(0, data);
                data = data.Select(x => UnwrapObject(x)).ToList();
                chart.SetData(data);
            }


        }

        public void OnBrush(IEnumerable<object> data, IEnumerable<int> idx)
        {
            result = data;
            indices = idx;
            newResult = true;
            this.ExpireSolution(true);
        }

        private object UnwrapObject(object obj)
        {
            if (obj is Grasshopper.Kernel.Types.GH_ObjectWrapper)
                return ((Grasshopper.Kernel.Types.GH_ObjectWrapper)obj).Value;
            else if (obj is Grasshopper.Kernel.Types.GH_String)
                return ((Grasshopper.Kernel.Types.GH_String)obj).Value;
            else if (obj is Grasshopper.Kernel.Types.IGH_Goo)
            {
                try
                {
                    System.Reflection.PropertyInfo prop = obj.GetType().GetProperty("Value");
                    return prop.GetValue(obj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Grasshopper sucks, what can I do?" + e.ToString());
                }
                return obj;

            }
            else
                return obj;
        }

        private D3T.BubbleChart chart = null;
        private IEnumerable<object> result;
        private IEnumerable<int> indices;
        private bool newResult;
    }
}
