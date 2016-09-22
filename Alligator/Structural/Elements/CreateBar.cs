using BHoM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using GHE = Grasshopper_Engine;
using Alligator.Components;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using BHP = BHoM.Structural.Properties;
using BHG = BHoM.Geometry;
using System.Windows.Forms;
using R = Rhino.Geometry;
using Grasshopper;
using GHKT = Grasshopper.Kernel.Types;
using Grasshopper_Engine.Components;
using ASP = Alligator.Structural.Properties;

namespace Alligator.Structural.Elements
{
    
    public class CreateBar : GH_Component
    {
        public CreateBar() : base("Create Bar", "CreateBar", "Create a BH Bar object", "Structure", "Elements") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BHoM_Bar; }
        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("c5bcd137-c3f3-4e8c-876a-199194c8389c");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Section Property", "P", "The section property of the bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("Node/CL", "NCL", "Start node or centreline of the bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("Node2", "N2", "End node of the bar", GH_ParamAccess.item);
            //pManager.AddGenericParameter("Material", "M", "Material of the bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("Orientation angel", "O", "Orientationangle or vector", GH_ParamAccess.item);
            pManager.AddGenericParameter("Attributes", "A", "Attributes of the bar", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "N", "Name of the element", GH_ParamAccess.item);
            pManager.AddGenericParameter("Custom Data", "CD", "Custom data to add to the bar", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            pManager[3].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "B", "The created bar", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BHE.Bar bar = new BHE.Bar();

            BHP.SectionProperty secProp = GHE.DataUtils.GetGenericData<BHP.SectionProperty>(DA, 0);

            BHE.Node stNode;
            BHE.Node enNode;

            if (!GetEndNodes(DA, 1, out stNode, out enNode)) { return; }

            //BHoM.Materials.Material mat = GHE.DataUtils.GetGenericData<BHoM.Materials.Material>(DA, 3);

            object angOrVec = null;

            if (!DA.GetData(3, ref angOrVec)) { return; }

            double angle;

            if (!GetOrientationAngle(angOrVec, out angle))
            {
                return;
            }

            ASP.BarAttributesContainer att = GHE.DataUtils.GetGenericData<ASP.BarAttributesContainer>(DA, 4);

            if (att != null)
            {
                bar.Release = att.BarReleases;
                bar.FEAType = att.FEAType;
                bar.StructuralUsage = att.StructuralUsage;
            }


            string name = "";

            if (DA.GetData(5, ref name))
            {
                bar.Name = name;
            }

            Dictionary<string, object> customData = GHE.DataUtils.GetData<Dictionary<string, object>>(DA, 6);

            if (customData != null)
            {
                foreach (KeyValuePair<string, object> item in customData)
                {
                    bar.CustomData.Add(item.Key, item.Value);
                }
            }

            //Set the properties of the bar
            bar.StartNode = stNode;
            bar.EndNode = enNode;
            //bar.Material = mat;
            bar.SectionProperty = secProp;
            bar.OrientationAngle = angle;

            DA.SetData(0, bar);
        }

        private bool GetOrientationAngle(object inp, out double angle)
        {
            if (typeof(GHKT.GH_String).IsAssignableFrom(inp.GetType()))
            {
                string str = ((GHKT.GH_String)inp).Value;
                return double.TryParse(str, out angle);
            }

            if (typeof(GHKT.GH_Number).IsAssignableFrom(inp.GetType()))
            {
                angle = ((GHKT.GH_Number)inp).Value;
                return true;
            }

            if (typeof(GHKT.GH_Integer).IsAssignableFrom(inp.GetType()))
            {
                angle = ((GHKT.GH_Integer)inp).Value;
                return true;
            }

            angle = 0;
            return false;
        }

        /// <summary>
        /// Gets the end nodes for the bar. Converts various types of lines and curves to nodes and checks
        /// if the first input is either a point/node or a curve. If it is a curve the second input is ignored
        /// </summary>
        private bool GetEndNodes(IGH_DataAccess DA, int firstDAindex, out BHE.Node stNode, out BHE.Node enNode)
        {

            object ncl = null;

            //No data collected from first input
            if (!DA.GetData(firstDAindex, ref ncl))
            {
                stNode = null;
                enNode = null;
                return false;
            }

            //Constructs endnodes from rhino line
            if (typeof(GHKT.GH_Line).IsAssignableFrom(ncl.GetType()))
            {
                stNode = new BHE.Node(GHE.GeometryUtils.Convert(((GHKT.GH_Line)ncl).Value.From));
                enNode = new BHE.Node(GHE.GeometryUtils.Convert(((GHKT.GH_Line)ncl).Value.To));
                return true;
            }

            //Constructs end nodes from rhino curve
            if (typeof(GHKT.GH_Curve).IsAssignableFrom(ncl.GetType()))
            {
                stNode = new BHE.Node(GHE.GeometryUtils.Convert(((GHKT.GH_Curve)ncl).Value.PointAtStart));
                enNode = new BHE.Node(GHE.GeometryUtils.Convert(((GHKT.GH_Curve)ncl).Value.PointAtEnd));
                return true;
            }

            //Constructs end nodes from BHoM curve
            if (typeof(BHG.Curve).IsAssignableFrom(ncl.GetType()))
            {
                stNode = new BHE.Node(((BHG.Curve)ncl).StartPoint);
                enNode = new BHE.Node(((BHG.Curve)ncl).EndPoint);
                return true;
            }

            //Constructs end nodes from nodes or points
            if (GHE.DataUtils.GetNodeFromPointOrNode(DA, firstDAindex, out stNode))
            {
                return GHE.DataUtils.GetNodeFromPointOrNode(DA, firstDAindex + 1, out enNode);
            }

            stNode = null;
            enNode = null;
            return false;
        }

        //private bool GetNodeFromPointOrNode(IGH_DataAccess DA, int DAindex, out BHE.Node node)
        //{
        //    object n = null;

        //    //Grab input data
        //    if (!DA.GetData(DAindex, ref n))
        //    {
        //        node = null;
        //        return false;
        //    }

        //    //Gets node
        //    if (typeof(BHE.Node).IsAssignableFrom(n.GetType()))
        //    {
        //        node = (BHE.Node)n;
        //        return true;
        //    }

        //    //Gets node from Rhino point
        //    if (typeof(GHKT.GH_Point).IsAssignableFrom(n.GetType()))
        //    {
        //        node = new BHE.Node(GHE.GeometryUtils.Convert(((GHKT.GH_Point)n).Value));
        //        return true;
        //    }

        //    //Gets node from BHoM point
        //    if (typeof(BHG.Point).IsAssignableFrom(n.GetType()))
        //    {
        //        node = new BHE.Node((BHG.Point)n);
        //        return true;
        //    }

        //    node = null;
        //    return false;

        //}


    }
}
