/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.oM.Base;
using BH.UI.Grasshopper.Templates;
using BH.UI.Base;
using BH.UI.Base.Components;
using BH.Engine.Reflection;
using GH_IO.Serialization;
using BH.oM.UI;

namespace BH.UI.Grasshopper.Components
{
    public class CreateTypeComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateTypeCaller();


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void OnCallerModified(object sender, CallerUpdate update)
        {
            base.OnCallerModified(sender, update);

            // Adding a tag under the component
            Type type = Caller.SelectedItem as Type;
            if (type != null)
                Message = type.ToText();
        }

        /*******************************************/

        public override bool Read(GH_IReader reader)
        {
            bool success = base.Read(reader);

            // Adding a tag under the component
            Type type = Caller.SelectedItem as Type;
            if (type != null)
                Message = type.ToText();

            return success;
        }

        /*******************************************/
    }
}

