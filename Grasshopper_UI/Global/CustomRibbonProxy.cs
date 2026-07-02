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

using BH.Adapter;
using BH.oM.Data.Requests;
using BH.oM.UI;
using BH.UI.Base;
using BH.UI.Grasshopper.Components;
using BH.UI.Grasshopper.Templates;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using GH = Grasshopper;

namespace BH.UI.Grasshopper.Global
{
    /// <summary>
    /// An IGH_ObjectProxy that populates the Grasshopper ribbon from a BHoM CustomRibbonEntry
    /// without writing any .ghuser files to disk. An instance of this proxy is registered with
    /// GH_ComponentServer.AddProxy() so that Grasshopper picks it up during ribbon layout.
    /// </summary>
    public class CustomRibbonProxy : IGH_ObjectProxy
    {
        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        // Stable GUID that identifies the BHoM Custom Ribbon as a "library" in GH_ComponentServer.
        private static readonly Guid m_LibraryGuid = new Guid("7B6E9A3C-4D2F-4A1B-8C5E-1F3D7A9B2E6C");

        private readonly CustomRibbonEntry m_Entry;
        private readonly Bitmap m_Icon;
        private readonly GH_InstanceDescription m_Desc;
        private readonly Type m_CallerType;


        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        /// <summary>True when the proxy was successfully built from the CustomRibbonEntry (i.e. ItemJson was valid).</summary>
        public bool IsValid => m_CallerType != null && m_Desc != null;

        // IGH_ObjectProxy members
        public Guid Guid { get; }
        public GH_Exposure Exposure { get; set; }
        public IGH_InstanceDescription Desc => m_Desc;
        public Bitmap Icon => m_Icon;
        public Type Type => m_CallerType;
        public GH_ObjectType Kind => GH_ObjectType.CompiledObject;
        public Guid LibraryGuid => m_LibraryGuid;
        public bool SDKCompliant => true;
        public bool Obsolete => false;
        public string Location => string.Empty;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CustomRibbonProxy(CustomRibbonEntry entry)
        {
            m_Entry = entry;
            Guid = DeriveGuid(entry);
            Exposure = (GH_Exposure)Math.Pow(2, entry.GroupIndex);

            // Create a temporary caller purely to read back its Name, description
            // and the fallback icon, all of which depend on the Caller having accepted the item.
            try
            {
                Caller temp = Activator.CreateInstance(entry.CallerType) as Caller;
                if (temp != null)
                {
                    object obj = BH.Engine.Serialiser.Convert.FromJson(entry.ItemJson);
                    temp.SetItem(obj);

                    m_CallerType = temp.GetType();
                    m_Icon = entry.Icon ?? temp.Icon_24x24;
                    m_Desc = new GH_InstanceDescription(
                        name: temp.Name,
                        nickName: temp.Name,
                        description: temp.Description,
                        category: entry.TabName,
                        subCategory: entry.Category);
                }
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordWarning(e, $"Failed to initialise CustomRibbonProxy for item in tab '{entry?.TabName}', category '{entry?.Category}'.");
            }
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public IGH_ObjectProxy DuplicateProxy()
        {
            return new CustomRibbonProxy(m_Entry);
        }

        /*******************************************/

        public IGH_DocumentObject CreateInstance()
        {
            try
            {
                Caller caller = null;
                if (m_CallerType != null)
                    caller = Activator.CreateInstance(m_CallerType) as Caller;

                if (caller == null)
                    return null;

                IGH_DocumentObject obj = Instances.ComponentServer.EmitObject(caller.Id);

                if (obj != null)
                    (obj as IGH_InitCodeAware)?.SetInitCode(m_Entry.ItemJson);

                return obj;
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordWarning(e, $"Failed to create instance for custom ribbon item in tab '{m_Entry?.TabName}', category '{m_Entry?.Category}'.");
                return null;
            }
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        /// <summary>
        /// Derives a deterministic GUID from the item's tab name, category, and JSON payload so that
        /// the same settings file always produces the same proxy GUID (important for canvas serialisation).
        /// Uses MD5 purely as a byte-stable hash function — not for any security purpose.
        /// </summary>
        private static Guid DeriveGuid(CustomRibbonEntry item)
        {
            string seed = $"{item.TabName}|{item.Category}|{item.ItemJson}";
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(seed));
                return new Guid(hash);
            }
        }

        /*******************************************/
    }
}
