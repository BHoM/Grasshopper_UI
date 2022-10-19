using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GH = Grasshopper;
using Grasshopper.Kernel;

namespace BH.Engine.Grasshopper
{
    public static partial class Query
    {
        public static string CanvasID(this GH_Document document)
        {
            if (document == null)
                document = GH.Instances.ActiveCanvas.Document;
            if (document == null)
                return null;

            if (m_CanvasRunTimeIDs.ContainsKey(document.RuntimeID))
                return m_CanvasRunTimeIDs[document.RuntimeID];

            string newID = Guid.NewGuid().ToString();
            m_CanvasRunTimeIDs.Add(document.RuntimeID, newID);
            return newID;
        }

        private static Dictionary<ulong, string> m_CanvasRunTimeIDs = new Dictionary<ulong, string>();
    }
}
