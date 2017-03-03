using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper_Engine;
using Grasshopper.Kernel;
using BHE = BHoM.Structural.Elements;
//using Autodesk.Revit;
//using ADG = Autodesk.DesignScript.Geometry;
//using Autodesk.Revit.DB;

namespace Revit_Alligator
{

   
        public class Beam : GH_Component
    {
        public Beam() : base("Revit Beam", "RevitBeam", "Create a Beam in Revit", "Structure", "Cable Net")
        { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("45BE085B-55E4-43A1-BBE5-5E9A496E219D");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "B", "Bar to Revit", GH_ParamAccess.item);
            pManager.AddTextParameter("Family Type Name", "FamilyType", "The name of the revit family type", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Beam Successfully created", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string typename = "";

            BHE.Bar bar = DataUtils.GetGenericData<BHE.Bar>(DA, 0);
            DA.GetData<string>(1, ref typename);

            bool success = CableNetDesign_Revit.ImportBeamTest.ImportBeam(bar, typename);


            DA.SetData(0, success);
           // string typename = DataUtils.GetGenericData<string>(DA, 1);


            //Autodesk.Revit.Creation.Document doc = Autodesk.Revit.Creation.Document
                
        //    Autodesk.Revit.DB.Document doc = RevitServices.Persistence.DocumentManager.Instance.CurrentDBDocument;

        //    RevitServices.Transactions.TransactionManager.Instance.EnsureInTransaction(doc);

        //    //Autodesk.Revit.DB.Element elem = 

        ////    ADG.Line cl = Geometry.BHLine.ToDSLine(bar.Line);

        //    //Revit.Elements.StructuralFraming.BeamByCurve(cl)

        //    //Autodesk.Revit.DB.FamilyInstance inst = new Autodesk.Revit.DB.FamilyInstance

        //    //Autodesk.Revit.DB.Line cl = Autodesk.Revit.DB.GeometryCreationUtilities
            
                

        //    //ADG.Point stPt = ADG.Point.ByCoordinates(bar.StartPoint.X, bar.StartPoint.Y, bar.StartPoint.Z);

        //    //ADG.Point endPt = ADG.Point.ByCoordinates(bar.EndPoint.X, bar.EndPoint.Y, bar.EndPoint.Z);

        //    //ADG.Line cl = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(stPt, endPt);


        //    BHoM.Structural.Properties.SteelSection secProp = (BHoM.Structural.Properties.SteelSection)bar.SectionProperty;

        //    string typeName = "RSS " + secProp.TotalDepth * 1000 + "x" + secProp.TotalWidth * 1000;

        //    Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeName);
            

        //    Level level = Level.Create(doc,0);

        //    Autodesk.Revit.DB.Structure.StructuralType structuraltype = Autodesk.Revit.DB.Structure.StructuralType.Beam;


        //    XYZ pt1 = new Autodesk.Revit.DB.XYZ(bar.StartPoint.X, bar.StartPoint.Y, bar.StartPoint.Z);
        //    XYZ pt2 = new Autodesk.Revit.DB.XYZ(bar.EndPoint.X, bar.EndPoint.Y, bar.EndPoint.Z);

        //    Curve cl = Line.CreateBound(pt1,pt2);


        //    FilteredElementCollector familySymbolCollector = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
        //    List<Autodesk.Revit.DB.FamilySymbol> documentFamilyTypes = familySymbolCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);
        //    Autodesk.Revit.DB.FamilySymbol familyParentType = documentFamilyTypes.Find(x => x.Name == typename);

            
        //    doc.Create.NewFamilyInstance(cl, familyParentType, level, structuraltype);


        //    RevitServices.Transactions.TransactionManager.Instance.TransactionTaskDone();
        //    // RevitServices.Elements.

            ////Autodesk.Revit.DB.FamilyInstance.
            ////Autodesk.Revit.DB.FamilyInstance inst = revbeam as Autodesk.Revit.DB.FamilyInstance;

            ////Autodesk.Revit.DB.Element elem = (Autodesk.Revit.DB.Element)revbeam;

        }

    }
}
