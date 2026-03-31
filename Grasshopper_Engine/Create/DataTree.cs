/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using BH.oM.Base.Attributes;
using Grasshopper;
using Grasshopper.Kernel.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Grasshopper
{
    public static partial class Create
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        [Description("Creates a Grasshopper DataTree from a list of lists, organising data under paths based on the iteration index.")]
        [Input("data", "The list of lists to organise into a data tree.")]
        [Input("iteration", "The iteration index used as the first element of each path.")]
        [Output("dataTree", "The created Grasshopper DataTree with each element placed under a path combining the iteration index and item index.")]
        public static DataTree<T> DataTree<T>(List<IEnumerable<T>> data, int iteration)
        {
            DataTree<T> tree = new DataTree<T>();

            if (data.Count == 0)
                tree.EnsurePath(new GH_Path(iteration));

            for (int i = 0; i < data.Count; i++)
            {
                tree.AddRange(data[i], new GH_Path(iteration, i));
            }

            return tree;
        }

        /*******************************************/

        [Description("Creates a Grasshopper DataTree from a list of lists, organising data under the provided paths based on the iteration index.")]
        [Input("data", "The list of lists to organise into a data tree.")]
        [Input("iteration", "The index used to select the path from the provided paths list.")]
        [Input("paths", "The list of pre-defined paths to organise data under.")]
        [Output("dataTree", "The created Grasshopper DataTree with data organised under the provided paths, or an empty tree if data is empty.")]
        public static DataTree<T> DataTree<T>(List<IEnumerable<T>> data, int iteration, IList<GH_Path> paths)
        {
            DataTree<T> master = new DataTree<T>();
            if (data.Count == 0)
            {
                master.EnsurePath(0);
                return new DataTree<T>();
            }
            else
            {
                for (int i = 0; i < data.Count; i++)
                {
                    DataTree<T> local = new DataTree<T>(data[i], paths[iteration]);
                    GH_Path path = paths[iteration].AppendElement(i);
                    master.EnsurePath(path);
                    master.AddRange(data[i], path);
                }
            }
            return master;
        }

        /*******************************************/
    }
}







