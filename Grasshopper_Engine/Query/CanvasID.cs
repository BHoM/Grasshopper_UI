/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.Text;
using System.Threading.Tasks;

using GH = Grasshopper;
using Grasshopper.Kernel;
using System.ComponentModel;
using BH.oM.Base.Attributes;

namespace BH.Engine.Grasshopper
{
    public static partial class Query
    {
        [Description("Gets a unique ID for a Grasshopper document based on the runtime ID of the document linked to a GUID. Valid only for the active session and will not be the same across sessions. Reloading Grasshopper will produce new IDs for the same files on a new load. This is predominantly used for Analytics.")]
        [Input("document", "A Grasshopper document to obtain a unique GUID from for the given runtime ID.")]
        [Output("canvasID", "A GUID set as a string to uniquely identify the session instance of the canvas.")]
        public static string CanvasID(this GH_Document document)
        {
            if (document == null)
                document = GH.Instances.ActiveCanvas.Document;
            if (document == null)
                return null;

            ulong documentKey = GetDocumentID(document);                

            if (m_CanvasRunTimeIDs.ContainsKey(documentKey))
                return m_CanvasRunTimeIDs[documentKey];

            string newID = Guid.NewGuid().ToString();
            m_CanvasRunTimeIDs.Add(documentKey, newID);
            return newID;
        }

        private static ulong GetDocumentID(GH_Document document)
        {
            if (document.Owner != null)
                return GetDocumentID(document.Owner.OwnerDocument()); //Recursive up to the highest level of a document

            return document.RuntimeID;
        }

        private static Dictionary<ulong, string> m_CanvasRunTimeIDs = new Dictionary<ulong, string>();
    }
}


