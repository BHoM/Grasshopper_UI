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

using BH.oM.Geometry;
using BH.UI.Templates;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Parameters;

namespace BH.UI.Grasshopper.Templates
{
    public class DataAccessor_GH : DataAccessor
    {
        /*************************************/
        /**** Properties                  ****/
        /*************************************/

        public IGH_DataAccess GH_Accessor { get; set; } = null;

        public GH_Component Component { get; set; } = null;


        /*************************************/
        /**** Properties                  ****/
        /*************************************/

        public DataAccessor_GH(GH_Component component)
        {
            Component = component;
        }


        /*************************************/
        /**** Input Getter Methods        ****/
        /*************************************/

        public override T GetDataItem<T>(int index)
        {
            if (GH_Accessor == null)
                return default(T);

            IGH_Goo goo = null;
            GH_Accessor.GetData(index, ref goo);

            IGH_TypeHint hint = null;
            Param_ScriptVariable scriptParam = Component.Params.Input[index] as Param_ScriptVariable;
            if (scriptParam != null)
                hint = scriptParam.TypeHint;

            return BH.Engine.Grasshopper.Convert.IFromGoo<T>(goo, hint);
        }

        /*************************************/

        public override List<T> GetDataList<T>(int index)
        {
            return GetDataCollection<T>(index).ToList();
        }

        /*************************************/

        public override T[] GetDataArray<T>(int index)
        {
            return GetDataCollection<T>(index).ToArray();
        }

        /*************************************/

        public override List<List<T>> GetDataTree<T>(int index)
        {
            if (GH_Accessor == null || Component == null)
                return new List<List<T>>();

            IGH_TypeHint hint = null;
            Param_ScriptVariable scriptParam = Component.Params.Input[index] as Param_ScriptVariable;
            if (scriptParam != null)
                hint = scriptParam.TypeHint;

            IGH_Param param = Component.Params.Input[index];
            return param.VolatileData.StructureProxy.Select(x => x.Cast<IGH_Goo>().Select(y => BH.Engine.Grasshopper.Convert.IFromGoo<T>(y, hint)).ToList()).ToList();

            // This shows that using the GetDataTree method from GH is actually giving the exact same result with the exact same problem of collecting the entire data instead of a subtree
            /*IGH_Structure goo = Activator.CreateInstance(typeof(GH_Structure<>).GetGenericTypeDefinition().MakeGenericType(new Type[] { param.Type })) as IGH_Structure;
            return ConvertDataTree(goo as dynamic, index, new List<List<T>>());*/
        }

        /*************************************/
        /**** Output Setter Methods       ****/
        /*************************************/

        public override bool SetDataItem<T>(int index, T data)
        {
            return GH_Accessor.SetData(index, data);
        }

        /*************************************/

        public override bool SetDataList<T>(int index, IEnumerable<T> data)
        {
            return GH_Accessor.SetDataList(index, (IEnumerable<T>)data);
        }

        /*************************************/

        public override bool SetDataTree<T>(int index, IEnumerable<IEnumerable<T>> data)
        {
            IGH_Param root;

            if (Component.IsValidPrincipalParameterIndex & Component.PrincipalParameterIndex != -1)
                root = Component.Params.Input[Component.PrincipalParameterIndex];
            else
                root = Component.Params.Input.FindAll(p => p.Access == GH_ParamAccess.tree).FirstOrDefault() ?? Component.Params.Input.First();

            return GH_Accessor.SetDataTree(index, BH.Engine.Grasshopper.Create.DataTree(data.ToList(), GH_Accessor.Iteration, root.VolatileData.Paths));
        }


        /*************************************/
        /**** Private Methods             ****/
        /*************************************/

        private List<List<T>> ConvertDataTree<T, TG>(GH_Structure<TG> structure, int index, List<List<T>> result) where TG : IGH_Goo
        {
            GH_Accessor.GetDataTree(index, out structure);
            result = structure.Branches.Select(x => x.Select(y => BH.Engine.Grasshopper.Convert.IFromGoo<T>(y)).ToList()).ToList();
            return result;
        }

        /*************************************/

        private IEnumerable<T> GetDataCollection<T>(int index)
        {
            if (GH_Accessor == null)
                return new T[0];

            IGH_TypeHint hint = null;
            Param_ScriptVariable scriptParam = Component.Params.Input[index] as Param_ScriptVariable;
            if (scriptParam != null)
                hint = scriptParam.TypeHint;

            List<IGH_Goo> goo = new List<IGH_Goo>();
            GH_Accessor.GetDataList<IGH_Goo>(index, goo);
            return goo.Select(x => BH.Engine.Grasshopper.Convert.FromGoo<T>(x, hint));
        }

        /*************************************/
    }
}
