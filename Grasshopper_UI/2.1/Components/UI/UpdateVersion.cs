/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using BH.UI.Grasshopper.Templates;
using System.IO;
using Grasshopper.Kernel.Special;
using BH.UI.Grasshopper.Components;
using System.Drawing;
using BH.UI.Grasshopper.Adapter;
using BH.UI.Components;
using BH.UI.Grasshopper.Parameters;

namespace BH.UI.Grasshopper.Base
{
    public class UpdateVersionComponent : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Guid ComponentGuid { get; } = new Guid("7DE8F586-ECC2-46DE-B8F7-3280151DBDD4");

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.CreateBHoM; 

        public override GH_Exposure Exposure { get; } = GH_Exposure.primary;

        public override bool Obsolete { get; } = false;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public UpdateVersionComponent() : base("Update GH File to New Version", "UpdateVersion", "Update Grasshopper file to the new version of the BHoM UI", "BHoM", "UI") {}


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("filePath", "filePath", "location of the file to update", GH_ParamAccess.item);
            pManager.AddTextParameter("newFileName", "newFileName", "name to give to teh new file", GH_ParamAccess.item);
            pManager.AddBooleanParameter("active", "active", "Execute the component", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("success", "success", "conversion was successful?", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Engine.Reflection.Compute.ClearCurrentEvents();

            string filePath = ""; DA.GetData(0, ref filePath);
            string newFileName = ""; DA.GetData(1, ref newFileName);
            bool active = false; DA.GetData(2, ref active);

            if (active)
            {
                bool result = UpdateToVersion2(filePath, newFileName);
                DA.SetData(0, result);
            }

            Logging.ShowEvents(this, BH.Engine.Reflection.Query.CurrentEvents());
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static bool UpdateToVersion2(string filePath, string newFileName)
        {
            // Make sure that the file exists
            if (!File.Exists(filePath))
            {
                Engine.Reflection.Compute.RecordError("The file does not exist.");
                return false;
            }

            // Open the Grasshopper file
            GH_DocumentIO docIO = new GH_DocumentIO();
            GH_Document doc = null;
            try
            {
                docIO.Open(filePath);
                doc = docIO.Document;
            }
            catch (Exception e)
            {
                Engine.Reflection.Compute.RecordError("Failed to open the Grasshopper file.\nError: " + e.Message);
                return false;
            }

            // Replace all the osolete components
            bool allOk = true;
            List<IGH_DocumentObject> obsoleteComponents = doc.Objects.Where(x => m_ComponentGuidMatch.ContainsKey(x.ComponentGuid)).ToList();
            foreach (IGH_DocumentObject component in obsoleteComponents)
                allOk &= ReplaceObsolete(doc, component as dynamic);

            // Save the file
            if (!newFileName.EndsWith(".gh"))
                newFileName += ".gh";
            doc.FilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);
            docIO.Save();

            // Return success
            return allOk;
        }

        /*******************************************/

        private static bool CopyAllLinks(GH_Component component, GH_Component newComp)
        {
            // Recreate the input links
            bool allOk = true;
            if (component.Params.Input.Count <= newComp.Params.Input.Count)
            {
                for (int i = 0; i < component.Params.Input.Count; i++)
                    CopyInputLinks(component.Params.Input[i], newComp.Params.Input[i]);
            }
            else
            {
                allOk = false;
                BH.Engine.Reflection.Compute.RecordError("Component " + component.NickName + " does not have the same number of inputs than its new version. The links have not been created.");
            }

            // Recreate the output links
            if (component.Params.Output.Count <= newComp.Params.Output.Count)
            {
                for (int i = 0; i < component.Params.Output.Count; i++)
                    CopyOutputLinks(component.Params.Output[i], newComp.Params.Output[i]);
            }
            else
            {
                allOk = false;
                BH.Engine.Reflection.Compute.RecordError("Component " + component.NickName + " does not have the same number of outputs than its new version. The links have not been created.");
            }

            return allOk;
        }

        /*******************************************/

        private static bool CopyInputLinks(IGH_Param source, IGH_Param target)
        {
            foreach (IGH_Param param in source.Sources)
                target.AddSource(param);

            return true;
        }

        /*******************************************/

        private static bool CopyOutputLinks(IGH_Param source, IGH_Param target)
        {
            foreach (IGH_Param param in source.Recipients)
                param.AddSource(target);

            return true;
        }

        /*******************************************/

        private static bool SwapComponent(GH_Document doc, GH_Component component, CallerComponent newComp, object selectedItem = null)
        {
            // Add the new component
            doc.AddObject(newComp, false);
            newComp.Attributes.Pivot = component.Attributes.Pivot;

            // Set the item of the component if exists
            if (selectedItem != null)
            {
                newComp.Caller.SetItem(selectedItem);
                newComp.OnItemSelected();
            }
                
            // Copy the links over
            bool allOk = CopyAllLinks(component, newComp);

            // Delete the old one
            if (allOk)
                doc.RemoveObject(component, false);
            else
                newComp.Attributes.Pivot = new PointF(newComp.Attributes.Pivot.X, newComp.Attributes.Pivot.Y + 10);

            return true;
        }

        /*******************************************/

        private static bool SwapComponent(GH_Document doc, GH_ValueList component, CallerValueList newComp, object selectedCategory = null)
        {
            // Add the new component
            doc.AddObject(newComp, false);
            newComp.Attributes.Pivot = component.Attributes.Pivot;

            // Set the category of the component if exists
            if (selectedCategory != null)
            {
                newComp.Caller.SetItem(selectedCategory);
                newComp.UpdateFromSelectedItem();
            }
                
            // Copy the selection over
            if (component.ListItems.Count > 0)
                newComp.SelectItem(component.ListItems.IndexOf(component.FirstSelectedItem));
                
            // Recreate the output links
            bool allOk = CopyOutputLinks(component, newComp);

            // Delete the old one
            if (allOk)
                doc.RemoveObject(component, false);
            else
                newComp.Attributes.Pivot = new PointF(newComp.Attributes.Pivot.X, newComp.Attributes.Pivot.Y + 10);

            return true;
        }


        /*******************************************/
        /**** Component Replacement Methods     ****/
        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, IGH_DocumentObject component)
        {
            // Fallback case. Do nothing
            return true;
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, CreateAdapter component)
        {
            return SwapComponent(doc, component, new CreateAdapterComponent(), component.SelectedItem);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, CreateQuery component)
        {
            return SwapComponent(doc, component, new CreateQueryComponent(), component.SelectedItem);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, Delete component)
        {
            return SwapComponent(doc, component, new DeleteComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, Execute component)
        {
            return SwapComponent(doc, component, new ExecuteComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, Move component)
        {
            return SwapComponent(doc, component, new MoveComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, Pull component)
        {
            return SwapComponent(doc, component, new PullComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, Push component)
        {
            return SwapComponent(doc, component, new PushComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, UpdateProperty component)
        {
            return SwapComponent(doc, component, new UpdatePropertyComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, ComputeBHoM component)
        {
            return SwapComponent(doc, component, new ComputeComponent(), component.SelectedItem);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, ConvertBHoM component)
        {
            return SwapComponent(doc, component, new ConvertComponent(), component.SelectedItem);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, ExplodeJson component)
        {
            ExplodeComponent newComp = new ExplodeComponent();

            foreach (IGH_Param param in component.Params.Output)
                newComp.Params.RegisterOutputParam(GetNewParam(param as dynamic));

            return SwapComponent(doc, component, newComp);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, FromJson component)
        {
            return SwapComponent(doc, component, new FromJsonComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, GetProperty component)
        {
            return SwapComponent(doc, component, new GetPropertyComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, ModifyBHoM component)
        {
            return SwapComponent(doc, component, new ModifyComponent(), component.SelectedItem);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, QueryBHoM component)
        {
            return SwapComponent(doc, component, new QueryComponent(), component.SelectedItem);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, SetProperty component)
        {
            return SwapComponent(doc, component, new SetPropertyComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, ToJson component)
        {
            return SwapComponent(doc, component, new ToJsonComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, CreateCustomObject component)
        {
            CreateCustomComponent newComp = new CreateCustomComponent();
            CreateCustomCaller caller = newComp.Caller as CreateCustomCaller;
            caller.SetInputs(component.Params.Input.Select(x => x.NickName).ToList());
            newComp.OnItemSelected();

            return SwapComponent(doc, component, newComp, component.SelectedType);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, CreateBHoMData component)
        {
            return SwapComponent(doc, component, new CreateDataComponent(), component.SelectedCategory);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, CreateDictionary component)
        {
            return SwapComponent(doc, component, new CreateDictionaryComponent());
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, CreateBHoMEnum component)
        {
            return SwapComponent(doc, component, new CreateEnumComponent(), component.SelectedType);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, CreateBHoM component)
        {
            return SwapComponent(doc, component, new CreateObjectComponent(), component.SelectedItem);
        }

        /*******************************************/

        private static bool ReplaceObsolete(GH_Document doc, CreateBHoMType component)
        {
            return SwapComponent(doc, component, new CreateTypeComponent(), component.SelectedType);
        }


        /*******************************************/
        /**** Param Replacement Methods         ****/
        /*******************************************/

        private static IGH_Param GetNewParam(IGH_Param param)
        {
            IGH_Param newParam = Activator.CreateInstance(param.GetType()) as IGH_Param;
            newParam.Name = param.Name;
            newParam.NickName = param.NickName;
            newParam.Access = param.Access;

            return newParam;
        }

        /*******************************************/

        private static IGH_Param GetNewParam(BHoMGeometryParameter param)
        {
            return new Param_BHoMGeometry { Name = param.Name, NickName = param.NickName, Access = param.Access };
        }

        /*******************************************/

        private static IGH_Param GetNewParam(BHoMObjectParameter param)
        {
            return new Param_BHoMObject { Name = param.Name, NickName = param.NickName, Access = param.Access };
        }

        /*******************************************/

        private static IGH_Param GetNewParam(DictionaryParameter param)
        {
            return new Param_Dictionary { Name = param.Name, NickName = param.NickName, Access = param.Access };
        }

        /*******************************************/

        private static IGH_Param GetNewParam(EnumParameter param)
        {
            return new Param_Enum { Name = param.Name, NickName = param.NickName, Access = param.Access };
        }

        /*******************************************/

        private static IGH_Param GetNewParam(IObjectParameter param)
        {
            return new Param_IObject { Name = param.Name, NickName = param.NickName, Access = param.Access };
        }

        /*******************************************/

        private static IGH_Param GetNewParam(TypeParameter param)
        {
            return new Param_Type { Name = param.Name, NickName = param.NickName, Access = param.Access };
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private static Dictionary<Guid, Guid> m_ComponentGuidMatch = new Dictionary<Guid, Guid>
        {
            {new Guid("a2d956ae-98a8-486d-a2ab-371b45f8b3ae"), new Guid("dd286cb5-2bc6-4c4a-aac5-542d1d0954b5") }, // CreateAdapter
            {new Guid("e1bc4c14-9f5b-4879-b8eb-ccac49178cfe"), new Guid("a4c4d4ba-8fb9-4ce5-802e-46a39b89fe5e") }, // CreateQuery
            {new Guid("8e2635f4-0c33-4608-910e-cdd676c03519"), new Guid("bf39598e-a021-4c52-8d65-20bc491b0bbd") }, // Delete
            {new Guid("1c89af97-379f-4432-b243-9c699eb454c2"), new Guid("d45ad8e8-cf03-464c-ba89-2122f4c6e4fa") }, // Execute
            {new Guid("a964110f-c8f5-4946-bae8-d829cf91d7ca"), new Guid("6d2c7f5b-7f64-47c8-ab69-424e5301582f") }, // Move
            {new Guid("ba3d716d-3044-4795-ac81-0fecc80781e3"), new Guid("b25011dd-5f30-4279-b9d9-0f9c169d6685") }, // Pull
            {new Guid("040cec18-c6e1-443b-b816-72b100304536"), new Guid("f27e94ad-6939-41aa-b680-094ba245f5c1") }, // Push
            {new Guid("e050834d-f825-4299-bea9-a3e067691925"), new Guid("33f6744b-ab9c-40b8-8606-479c6e10c2cc") }, // UpdateProperty

            {new Guid("9a94f1c4-af5b-48e6-b0dd-f56145deedda"), new Guid("a4ebe086-e659-4273-940b-98fd9bd73436") }, // Compute
            {new Guid("d517e0bf-e979-4441-896e-1d2ec833fe2e"), new Guid("dbb544eb-1edc-4ef0-a935-ea92ff989cf7") }, // Convert
            {new Guid("f2080175-a812-4dfb-86de-ae7dc8245668"), new Guid("3647c48a-3322-476f-8b34-4011540ab916") }, // Explode
            {new Guid("eb108fe0-a807-4cea-a8eb-2b8d54adbc04"), new Guid("d5d0ec6d-394b-4781-ac33-c278e1a77009") }, // FromJson
            {new Guid("e14ef77d-4f09-4cfb-ab75-f9b723212d00"), new Guid("c0bcb684-80e5-4a67-bf0e-6b8c2c917312") }, // GetProperty
            {new Guid("c275b1a2-bb2d-4f3b-8d5c-18c78456a831"), new Guid("2b79756e-c774-470b-8f62-0f20c4ae2dc8") }, // Modify
            {new Guid("63da0cac-87bc-48ac-9c49-1d1b2f06be83"), new Guid("2e60079c-3921-4c4f-8c44-7052c85fa36b") }, // Query
            {new Guid("e3c42f6c-15ac-4fba-8bcc-f3e773b1c1d8"), new Guid("a186d4f1-fc80-499b-8bbf-ecdd49bf6e6e") }, // SetProperty
            {new Guid("3564a67c-3444-4a9b-ae6b-591f1ca9a53a"), new Guid("fe8024d0-6db7-46fe-8785-75b25267fbe6") }, // ToJson

            {new Guid("dbd3fe50-423a-4ea4-8bc7-7ad94d1d67e9"), new Guid("eb7b72e5-b4d8-4ff6-bcbd-833cdec5d1a2") }, // CreateCustom
            {new Guid("947798c3-bc2b-466b-b450-571df8eea66c"), new Guid("b7325a7f-0465-45a4-9537-24a96a5a2fec") }, // CreateData
            {new Guid("6758eee1-6a49-4d2b-a7fd-974383d3622e"), new Guid("2df2a7fa-55d5-4ba6-8a1c-19bf2c555b04") }, // CreateDictionary
            {new Guid("68b29fae-057b-417a-96bc-32224974ccbe"), new Guid("f9c81693-ce16-456a-a1c4-aa109b6f56fe") }, // CreateEnum
            {new Guid("0e1c95eb-1546-47d4-89bb-776f7920622d"), new Guid("76221701-c5e7-4a93-8a2b-d34e77ed9cc1") }, // CreateObject
            {new Guid("fc00cd7c-aac6-43fc-a6b7-bbe35bf0e4fd"), new Guid("d51978f0-6beb-4832-9f65-db00de85c3b9") }  // CreateType
        };

        /*******************************************/
    }
}