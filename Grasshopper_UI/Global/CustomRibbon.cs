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

using BH.oM.UI;
using BH.UI.Base.Global;
using Grasshopper.Kernel;
using GH = Grasshopper;
using System;

namespace BH.UI.Grasshopper.Global
{
    public static class CustomRibbon
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static void Activate()
        {
            Initialisation.CustomRibbonEntryLoaded += OnCustomRibbonEntryLoaded;

            foreach (CustomRibbonEntry entry in Initialisation.CustomRibbonEntries)
                OnCustomRibbonEntryLoaded(null, entry);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static void OnCustomRibbonEntryLoaded(object sender, CustomRibbonEntry item)
        {
            try
            {
                CustomRibbonProxy proxy = new CustomRibbonProxy(item);

                if (!proxy.IsValid)
                {
                    BH.Engine.Base.Compute.RecordWarning($"Could not create a valid proxy for custom ribbon item. Tab: {item?.TabName}, Category: {item?.Category}. Check that the ItemJson is valid.");
                    return;
                }

                GH.Instances.ComponentServer.AddProxy(proxy);

                // UpdateRibbonUI is only needed when the ribbon has already been built (e.g. runtime reload).
                // During plugin startup the proxies are picked up automatically when the ribbon is built for the first time.
                if (GH.Instances.DocumentEditor != null)
                    GH_ComponentServer.UpdateRibbonUI();
            }
            catch (Exception e)
            {
                BH.Engine.Base.Compute.RecordWarning(e, $"Failed to register custom ribbon item. Tab: {item?.TabName}, Category: {item?.Category}.");
            }
        }

        /*******************************************/
    }
}
