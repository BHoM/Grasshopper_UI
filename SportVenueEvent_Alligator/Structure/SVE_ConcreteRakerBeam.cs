using System;
using BH.oM.Structural;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Grasshopper_Engine.Components;
using GHE = Grasshopper_Engine;

namespace BH.UI.Grasshopper.SportVenueEvent
{
    public class ConcreteRakerBeam : GH_Component
    {
        public ConcreteRakerBeam() : base("Concrete raker beam", "ConcreteRakerBeam", "Create a BH Concrete raker beam object", "SportVenueEvent", "Structural") { }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Profile", "P", "Profile", GH_ParamAccess.item);

            pManager.AddPointParameter("Location", "L", "Location of the Rakers", GH_ParamAccess.item);

            pManager.AddVectorParameter("Orientation", "O", "Orientation of the Raker", GH_ParamAccess.item);
            
            pManager.AddNumberParameter("Widht", "W", "Widht of the Raker", GH_ParamAccess.item);

            pManager.AddTextParameter("Name", "N", "Name of the Raker", GH_ParamAccess.item);

            pManager.AddGenericParameter("Bars", "B", "Bars of the Raker", GH_ParamAccess.item);
            pManager[5].Optional = true;

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Raker", "R", "Raker", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.oM.Structural.Elements.ConcreteRakerBeam CR = new BH.oM.Structural.Elements.ConcreteRakerBeam();
            CR.Profile = GHE.DataUtils.GetData<BHoM.Geometry.Polyline>(DA, 0);
            CR.Location = GHE.GeometryUtils.Convert(GHE.DataUtils.GetData<Rhino.Geometry.Point3d>(DA, 1));
            CR.Orientation = GHE.GeometryUtils.Convert(GHE.DataUtils.GetData<Rhino.Geometry.Vector3d>(DA, 2));
            CR.Width = GHE.DataUtils.GetData<double>(DA, 3);
            CR.Name = GHE.DataUtils.GetData<string>(DA, 4);


            if (GHE.DataUtils.GetData<BHoM.Structural.Elements.Bar>(DA, 5) != null)
            {
                List<BHoM.Structural.Elements.Bar> bars = new List<BHoM.Structural.Elements.Bar>();
                bars.Add(GHE.DataUtils.GetData<BHoM.Structural.Elements.Bar>(DA, 5));
                CR.Bars = bars;
            }
            DA.SetData(0, CR);
        }
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E50-410F-BBC7-C255FD1BD2B6");
            }
        }
    }
}