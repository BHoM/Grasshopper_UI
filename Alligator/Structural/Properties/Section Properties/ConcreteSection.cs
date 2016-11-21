using System;
using Grasshopper.Kernel;
using Alligator.Components;
using System.Collections.Generic;
using BHE = BHoM.Structural.Elements;
using BHoM.Structural.Properties;
using Grasshopper_Engine.Components;
using System.Windows.Forms;
using Grasshopper.Kernel.Parameters;
using System.Reflection;

namespace Alligator.Structural.Properties.Section_Properties
{
    public class CreateConcreteSection : CreateSectionProperty<ConcreteSection>
    {
        public CreateConcreteSection() : base("Create Concrete Section", "CreateConcreteSection", "Create a BH Section property object", "Structure", "Properties")
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the section Property", GH_ParamAccess.item);
            pManager.AddGenericParameter("CustomData", "CustomData", "CustomData", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "The material of the cross section", GH_ParamAccess.item);
            pManager.AddGenericParameter("Reinforcement", "Reinforcement", "List of reinforcement assigned to the concrete section", GH_ParamAccess.list);
            pManager.AddNumberParameter("Cover", "Cover", "Minimum cover to reinforcement layer", GH_ParamAccess.item);
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
            Params.Input[4].AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, 0.025);

            AppendEnumOptions("Shape", typeof(ShapeType));

            m_Options[0].Items.Clear();

            m_Options[0].Items.Add(ShapeType.Rectangle.ToString());
            m_Options[0].Items.Add(ShapeType.Circle.ToString());
            m_Options[0].Items.Add(ShapeType.Tee.ToString());
            m_Options[0].Items.Add(ShapeType.ISection.ToString());
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("95916777-2b86-46ab-b4e3-d839b817dbb4");
            }
        }
    }

    public class CreateLayerReinforcement : BHoMBaseComponent<LayerReinforcement>
    {
        public CreateLayerReinforcement() : base("Create Layer Reinforcement", "CreateLayerRein", "Create a BH Reinforcement layer", "Structure", "Properties")
        {

        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("95923777-2b86-46ab-b4e3-d839b817dbb4");
            }
        }
    }

    public class CreateTieReinforcement : BHoMBaseComponent<TieReinforcement>
    {
        public CreateTieReinforcement() : base("Create Tie Reinforcment", "CreateTieRein", "Create a BH Tie Reinforcement", "Structure", "Properties")
        {

        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("95923777-2b86-46ab-b433-d839b817dbb4");
            }
        }
    }



    public class CreatePerimeterReinforcement : BHoMBaseComponent<PerimeterReinforcement>
    {
        public CreatePerimeterReinforcement() : base("Create Perimeter Reinforcement", "CreatePerimRein", "Create a BH Perimeter Reinforcement layer", "Structure", "Properties")
        {

        }       

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("23923777-2b86-46ab-b4e3-d839b817dbb4");
            }
        }
    }
}
