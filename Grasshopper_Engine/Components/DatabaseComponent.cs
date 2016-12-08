using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GH_IO.Serialization;
using System.Drawing;
using Grasshopper.GUI.Canvas;
using System.Windows.Forms;

namespace Grasshopper_Engine.Components
{
    public class DBAttribute : GH_Attributes<DatabaseComponent>
    {
        public DBAttribute(DatabaseComponent owner)
            : base(owner)
        {

        }

        public override bool HasInputGrip
        {
            get
            {
                return false;
            }
        }

        public override bool HasOutputGrip
        {
            get
            {
                return true;
            }
        }

        protected override void Layout()
        {
            int width = GH_FontServer.StringWidth(Owner.TableName + Owner.ObjectName, GH_FontServer.Standard);
            width = Math.Max(width + 20, 60);

            int height = 20;

            Bounds = new RectangleF(Pivot, new SizeF(width, height));
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            Layout();

            if (channel == GH_CanvasChannel.Wires)
            {
                return;
            }

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Palette palette = GH_Palette.Normal;

                switch (Owner.RuntimeMessageLevel)
                {
                    case GH_RuntimeMessageLevel.Warning:
                        palette = GH_Palette.Warning;
                        break;

                    case GH_RuntimeMessageLevel.Error:
                        palette = GH_Palette.Error;
                        break;
                }

                int width1 = GH_FontServer.StringWidth(Owner.TableName, GH_FontServer.Standard) + 10;
                int width2 = GH_FontServer.StringWidth(Owner.ObjectName, GH_FontServer.Standard) + 10;

                RectangleF bound = new RectangleF(OutputGrip.X, OutputGrip.Y - 4, 8, 8);

                //OutputGrip
                graphics.FillPie(new SolidBrush(Color.White), bound.X, bound.Y, bound.Width, bound.Height, -90, 180);
                graphics.DrawArc(new Pen(new SolidBrush(Color.Black), 2), bound, -90, 180);

                RectangleF bounds1 = new RectangleF(Bounds.Location, new SizeF(width1, 20));

                RectangleF bounds2 = new RectangleF(new PointF(Bounds.Location.X + width1, Bounds.Location.Y), new SizeF(width2, 20));

                GH_Capsule capsule1 = GH_Capsule.CreateCapsule(bounds1, palette);
                GH_Capsule capsule2 = GH_Capsule.CreateCapsule(bounds2, GH_Palette.White);
                capsule1.Render(graphics, Selected, Owner.Locked, true);

                capsule1.Dispose();
                capsule1 = null;

                capsule2.Render(graphics, Selected, Owner.Locked, true);

                capsule2.Dispose();
                capsule2 = null;

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;

                graphics.DrawString(Owner.TableName, GH_FontServer.Standard, Brushes.Black, bounds1, format);
                graphics.DrawString(Owner.ObjectName, GH_FontServer.Standard, Brushes.Black, bounds2, format);

                format.Dispose();
            }
        }
    }


    public abstract class DatabaseComponent : GH_Param<GH_ObjectWrapper>
    {
        protected BHoM.Base.Database m_DatabaseType;
        public string ObjectType { get { return m_Types.Text.Trim(); } }
        public string ObjectName { get { return m_Names.Text.Trim(); } }
        public string TableName { get { return m_Tables.Text.Trim(); } }

        private ComboBox m_Tables;
        private ComboBox m_Types;
        private ComboBox m_Names;

        public DatabaseComponent(string name, string nickname, string description, string category, string subCategory) 
            : base(name, nickname, description, category, subCategory, GH_ParamAccess.item)
        {
            CreateMenus();
        }

        public void CreateMenus()
        {
            m_Tables = new ComboBox();
            m_Types = new ComboBox();
            m_Names = new ComboBox();

            m_Tables.DropDownStyle = ComboBoxStyle.DropDownList;
            m_Types.DropDownStyle = ComboBoxStyle.DropDownList;
            m_Names.DropDownStyle = ComboBoxStyle.DropDownList;

            m_Tables.SelectedIndexChanged += M_Tables_SelectedIndexChanged;
            m_Types.SelectedIndexChanged += M_Types_SelectedIndexChanged;
            m_Names.SelectedIndexChanged += M_Names_SelectedIndexChanged;
        }

        public override void CreateAttributes()
        {
            m_attributes = new DBAttribute(this);
        }

        protected abstract void SetData();
        private void M_Names_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetData();
            this.ExpirePreview(true);     
        }

        private void M_Types_SelectedIndexChanged(object sender, EventArgs e)
        {
            BHoM.Base.SQLAccessor accessor = new BHoM.Base.SQLAccessor(m_DatabaseType, TableName);
            m_Names.Items.Clear();
            m_Names.Items.AddRange(accessor.GetDataColumn("Name", "where Type like '" + ObjectType +"'").ToArray());
            m_Names.SelectedIndex = 0;
        }

        private void M_Tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            BHoM.Base.SQLAccessor accessor = new BHoM.Base.SQLAccessor(m_DatabaseType, TableName);
            m_Types.Items.Clear();
            m_Types.Items.AddRange(accessor.GetDataColumn("Type").Distinct().ToArray());
            m_Types.SelectedIndex = 0;
        }

        protected void Initialise(BHoM.Base.Database db)
        {
            BHoM.Base.SQLAccessor accessor = new BHoM.Base.SQLAccessor(db, "");
            
            m_DatabaseType = db;
            m_Tables.Items.AddRange(accessor.GetTableNames().ToArray());
            m_Tables.SelectedIndex = 0;
        } 

        public override Guid ComponentGuid
        {
            get { return new Guid("b34fee55-e2c3-4d4b-942c-b2694e043ac9"); }
        }

        public override bool AppendMenuItems(ToolStripDropDown menu)
        {
            if (m_Tables != null)
            {
                Menu_AppendObjectName(menu);// (menu, "Section");
                Menu_AppendSeparator(menu);
                Menu_AppendCustomItem(menu, m_Tables);
                Menu_AppendCustomItem(menu, m_Types);
                Menu_AppendCustomItem(menu, m_Names);
                Menu_AppendItem(menu, "Commit Changes", new EventHandler(On_CommitChanges));
                Menu_AppendItem(menu, "Cancel", new EventHandler(On_Cancel));
                Menu_AppendSeparator(menu);
                Menu_AppendSimplifyParameter(menu);
                Menu_AppendFlattenParameter(menu);
                Menu_AppendGraftParameter(menu);
                Menu_AppendReverseParameter(menu);
                // Menu
                Menu_AppendSeparator(menu);
                Menu_AppendObjectHelp(menu);
                return true;
            }
            else
                return base.AppendMenuItems(menu);
        }

        private void On_Cancel(object sender, EventArgs e)
        {

        }

        private void On_CommitChanges(object sender, EventArgs e)
        {
            this.ExpireSolution(true);
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (m_Tables != null)
            {
                writer.SetString("Table", m_Tables.Text);
                writer.SetString("Type", m_Types.Text);
                writer.SetString("Name", m_Names.Text);
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            CreateMenus();

            string tableName = "";
            string type = "";
            string name = "";

            if (reader.TryGetString("Table", ref tableName)) m_Tables.Text = tableName;
            if (reader.TryGetString("Type", ref type)) m_Types.Text = type;
            if (reader.TryGetString("Name", ref name)) m_Names.Text = name;


            return base.Read(reader);
        }
    }
}
