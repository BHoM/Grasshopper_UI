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
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Collections;

namespace BH.UI.Grasshopper.Base
{
    public class CreateDictionary : CreateCustomTemplate
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.Dictionary; 

        public override Guid ComponentGuid { get; } = new Guid("6758EEE1-6A49-4D2B-A7FD-974383D3622E"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.hidden;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateDictionary() : base("Create Dictionary", "Dictionary", "Create a dictionary from a list of keys and values", "Grasshopper", " oM") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        /*******************************************/

        public override bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("Keys", "Keys", "list of keys for the dictionary", GH_ParamAccess.list);
            pManager.AddScriptVariableParameter("Values", "Values", "list of values for the dictionary", GH_ParamAccess.list);

            base.RegisterInputParams(pManager);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new DictionaryParameter(), "D", "D", "Dictionary", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<object> keys = GetListFromParameter(DA, 0);
            List<object> values = GetListFromParameter(DA, 1);

            if (keys.Count > 0 && values.Count == keys.Count)
            {
                Type keyType = keys.First().GetType();
                Type valueType = values.First().GetType();

                Type dicType = typeof(Dictionary<,>).MakeGenericType(new Type[] { keyType, valueType });
                IDictionary dic = (IDictionary) Activator.CreateInstance(dicType);
                for (int i = 0; i < keys.Count; i++)
                    dic.Add(keys[i], values[i]);

                DA.SetData(0, dic);
            }
        }

        /*******************************************/
    }
}
