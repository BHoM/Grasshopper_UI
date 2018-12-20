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

using BH.Engine.Reflection;
using BH.Engine.Serialiser;
using BH.oM.Base;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GH = Grasshopper;


namespace BH.UI.Grasshopper.Menus
{
   
    public class BH_PopupSearchDialog : Form
    {
        private class GH_Hit
        {
            public IGH_ObjectProxy proxy;

            public Rectangle region;

            public string name;

            public MethodInfo method;

            public GH_Hit(IGH_ObjectProxy nProxy, string nName, MethodInfo nMethod)
            {
                proxy = nProxy;
                name = nName;
                method = nMethod;
            }

            public GH_Hit(IGH_ObjectProxy nProxy, string nName, MethodInfo nMethod, Rectangle nRegion)
            {
                proxy = nProxy;
                name = nName;
                method = nMethod;
                region = nRegion;
            }
        }

        private IContainer components;

        [AccessedThroughProperty("txtSearch")]
        private TextBox _txtSearch;

        [AccessedThroughProperty("pnlHits")]
        private GH_DoubleBufferedPanel _pnlHits;

        [AccessedThroughProperty("ToolTip")]
        private ToolTip _ToolTip;

        [CompilerGenerated]
        private System.Drawing.Point _BasePoint;

        [CompilerGenerated]
        private GH_Canvas _Canvas;

        private bool _inserted;

        private int m_selected_index;

        private List<GH_Hit> m_hits;

        private Dictionary<string, MethodInfo> m_MethodList;

        private string m_message;


        private static Dictionary<string, IGH_ObjectProxy> m_ProxyDict = new Dictionary<string, IGH_ObjectProxy>
        {
            { "Create", GH.Instances.ComponentServer.EmitObjectProxy(new Guid("0E1C95EB-1546-47D4-89BB-776F7920622D")) },
            { "Convert", GH.Instances.ComponentServer.EmitObjectProxy(new Guid("D517E0BF-E979-4441-896E-1D2EC833FE2E")) },
            { "Modify", GH.Instances.ComponentServer.EmitObjectProxy(new Guid("C275B1A2-BB2D-4F3B-8D5C-18C78456A831")) },
            { "Compute", GH.Instances.ComponentServer.EmitObjectProxy(new Guid("9A94F1C4-AF5B-48E6-B0DD-F56145DEEDDA")) },
            { "Query", GH.Instances.ComponentServer.EmitObjectProxy(new Guid("63DA0CAC-87BC-48AC-9C49-1D1B2F06BE83")) }
        };

        internal virtual TextBox txtSearch
        {
            get
            {
                return _txtSearch;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                System.EventHandler value2 = new System.EventHandler(txtSearch_TextChanged);
                System.EventHandler value3 = new System.EventHandler(txtSearch_LostFocus);
                KeyEventHandler value4 = new KeyEventHandler(txtSearch_KeyDown);
                if (_txtSearch != null)
                {
                    _txtSearch.TextChanged -= value2;
                    _txtSearch.LostFocus -= value3;
                    _txtSearch.KeyDown -= value4;
                }
                _txtSearch = value;
                if (_txtSearch != null)
                {
                    _txtSearch.TextChanged += value2;
                    _txtSearch.LostFocus += value3;
                    _txtSearch.KeyDown += value4;
                }
            }
        }

        internal virtual GH_DoubleBufferedPanel pnlHits
        {
            get
            {
                return _pnlHits;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                MouseEventHandler value2 = new MouseEventHandler(pnlHits_MouseMove);
                System.EventHandler value3 = new System.EventHandler(pnlHits_MouseLeave);
                MouseEventHandler value4 = new MouseEventHandler(pnlHits_MouseDown);
                PaintEventHandler value5 = new PaintEventHandler(pnlHits_Paint);
                if (_pnlHits != null)
                {
                    _pnlHits.MouseMove -= value2;
                    _pnlHits.MouseLeave -= value3;
                    _pnlHits.MouseDown -= value4;
                    _pnlHits.Paint -= value5;
                }
                _pnlHits = value;
                if (_pnlHits != null)
                {
                    _pnlHits.MouseMove += value2;
                    _pnlHits.MouseLeave += value3;
                    _pnlHits.MouseDown += value4;
                    _pnlHits.Paint += value5;
                }
            }
        }

        internal virtual ToolTip ToolTip
        {
            get
            {
                return _ToolTip;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _ToolTip = value;
            }
        }

        /// <summary>
        ///  The base point indicates where the popup is centered. 
        ///  This is typically the location of the cursor at the time the popup was created. 
        ///  The base point is located in the center of the search box.
        ///  </summary>
        public System.Drawing.Point BasePoint
        {
            get
            {
                return _BasePoint;
            }
            set
            {
                _BasePoint = value;
            }
        }

        public GH_Canvas Canvas
        {
            get
            {
                return _Canvas;
            }
            set
            {
                _Canvas = value;
            }
        }

        private int SelectedIndex
        {
            get
            {
                return m_selected_index;
            }
            set
            {
                value = System.Math.Max(value, 0);
                value = System.Math.Min(value, m_hits.Count - 1);
                m_selected_index = value;
            }
        }

        private GH_Hit SelectedHit
        {
            get
            {
                if (m_hits == null)
                {
                    return null;
                }
                if (m_hits.Count == 0)
                {
                    return null;
                }
                int i = SelectedIndex;
                if (i < 0)
                {
                    return null;
                }
                if (i >= m_hits.Count)
                {
                    return null;
                }
                return m_hits[i];
            }
        }

        private List<GH_Hit> HitList
        {
            get
            {
                return m_hits;
            }
        }

        public BH_PopupSearchDialog()
        {
            base.Load += new System.EventHandler(BH_PopupSearchDialog_Load);
            base.KeyDown += new KeyEventHandler(BH_PopupSearchDialog_KeyDown);
            _inserted = false;
            m_hits = new List<GH_Hit>();
            m_message = null;
            
            m_MethodList = new Dictionary<string, MethodInfo>();
            foreach (MethodInfo method in BH.Engine.Reflection.Query.BHoMMethodList().Where(x => !x.IsNotImplemented() && !x.IsDeprecated()))
            {
                try
                {
                    if (m_ProxyDict.ContainsKey(method.DeclaringType.Name))
                    {
                        string key = method.DeclaringType.Name + '.' + method.ToText(true);

                        if (m_MethodList.ContainsKey(key))
                            Console.WriteLine(key);

                        m_MethodList[key] = method;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ouch : " + e.ToString());
                }
            }
            

            InitializeComponent();
        }

        [System.Diagnostics.DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        private void InitializeComponent()
        {
            components = new Container();
            txtSearch = new TextBox();
            pnlHits = new GH_DoubleBufferedPanel();
            ToolTip = new ToolTip(components);
            SuspendLayout();
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Dock = DockStyle.Bottom;
            txtSearch.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Control arg_8D_0 = txtSearch;
            System.Drawing.Point location = new System.Drawing.Point(0, 165);
            arg_8D_0.Location = location;
            txtSearch.Name = "txtSearch";
            Control arg_B7_0 = txtSearch;
            Size size = new Size(200, 22);
            arg_B7_0.Size = size;
            txtSearch.TabIndex = 0;
            txtSearch.TextAlign = HorizontalAlignment.Center;
            txtSearch.WordWrap = false;
            pnlHits.Dock = DockStyle.Bottom;
            Control arg_FD_0 = pnlHits;
            location = new System.Drawing.Point(0, 66);
            arg_FD_0.Location = location;
            pnlHits.Name = "pnlHits";
            Control arg_127_0 = pnlHits;
            size = new Size(200, 99);
            arg_127_0.Size = size;
            pnlHits.TabIndex = 1;
            ToolTip.AutoPopDelay = 32000;
            ToolTip.InitialDelay = 500;
            ToolTip.ReshowDelay = 100;
            SizeF autoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleDimensions = autoScaleDimensions;
            AutoScaleMode = AutoScaleMode.Font;
            size = new Size(200, 187);
            ClientSize = size;
            ControlBox = false;
            Controls.Add(pnlHits);
            Controls.Add(txtSearch);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BH_PopupSearchDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.Manual;
            Text = "BH_PopupSearchDialog";
            ResumeLayout(false);
            PerformLayout();
        }

        private void BH_PopupSearchDialog_Load(object sender, System.EventArgs e)
        {
            GH_WindowsControlUtil.FixTextRenderingDefault(Controls);
            SetDefaultMessage();
        }

        private void BH_PopupSearchDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Cancel)
            {
                Close();
            }
        }

        private void LayoutPopupDialog()
        {
            int h = txtSearch.Height + pnlHits.Height;
            int x = BasePoint.X - Width / 2;
            int y = BasePoint.Y - txtSearch.Height / 2 - pnlHits.Height;
            SetBounds(x, y, Width, h);
        }

        private void pnlHits_MouseDown(object sender, MouseEventArgs e)
        {
            InsertComponent();
        }

        private void pnlHits_MouseLeave(object sender, System.EventArgs e)
        {
            GH_Tooltip.Clear();
        }

        private void pnlHits_MouseMove(object sender, MouseEventArgs e)
        {
            SelectedIndex = IndexUnderPixel(e.Location);
            pnlHits.Refresh();
            if (SelectedIndex < 0)
            {
                GH_Tooltip.Clear();
            }
            else
            {
                GH_Hit hit = SelectedHit;
                if (GH_Tooltip.IsTag(hit))
                {
                    GH_Tooltip.Adjust();
                }
                else
                {
                    txtSearch.LostFocus -= new System.EventHandler(txtSearch_LostFocus);
                    GH_Tooltip.AssignTooltipFields(hit.proxy.Desc.Name, hit.method.Description(), null, hit.proxy.Icon, null);
                    GH_Tooltip.Tag = hit;
                    GH_Tooltip.Show(txtSearch);
                    txtSearch.LostFocus += new System.EventHandler(txtSearch_LostFocus);
                }
            }
        }

        private void FadeOut()
        {
            Hide();
            if (GH.Instances.DocumentEditor != null)
            {
                GH.Instances.DocumentEditor.BringToFront();
            }
            Close();
        }

        private void InsertComponent()
        {
            if (_inserted)
            {
                return;
            }
            _inserted = true;
            if (Canvas == null)
            {
                FadeOut();
                return;
            }
            if (SelectedHit == null)
            {
                FadeOut();
                return;
            }
            System.Drawing.Point clientPoint = Canvas.PointToClient(BasePoint);
            System.Drawing.PointF canvasPoint = Canvas.Viewport.UnprojectPoint(clientPoint);
            string text = txtSearch.Text;
            text = text.Trim();
            if (text.Length == 0)
            {
                FadeOut();
                return;
            }

            CustomObject methodInfo = new CustomObject();
            methodInfo.CustomData = new Dictionary<string, object>
            {
                { "TypeName", SelectedHit.method.DeclaringType.AssemblyQualifiedName },
                { "MethodName", SelectedHit.method.Name },
                { "Parameters", SelectedHit.method.GetParameters().Select(x => x.ParameterType.AssemblyQualifiedName).ToList<object>() }
            };
            Canvas.InstantiateNewObject(SelectedHit.proxy.Guid, methodInfo.ToJson(), canvasPoint, true);
            FadeOut();
        }

        private int IndexUnderPixel(System.Drawing.Point pt)
        {
            int arg_0F_0 = 0;
            int num = m_hits.Count - 1;
            for (int i = arg_0F_0; i <= num; i++)
            {
                if (m_hits[i].region.Contains(pt))
                {
                    return i;
                }
            }
            return -1;
        }

        private void PopulateHitList(string key)
        {
            m_hits.Clear();
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            string[] terms = key.Split(new char[]
            {
                ' '
            }, System.StringSplitOptions.RemoveEmptyEntries);
            if (terms == null)
            {
                return;
            }
            if (terms.Length == 0)
            {
                return;
            }

            string text = key.ToLower();
            string[] parts = text.Split(' ');
            IEnumerable<KeyValuePair<string, MethodInfo>> hits = m_MethodList.Where(x => parts.All(y => x.Key.Substring(0, x.Key.IndexOf('(')+1).ToLower().Contains(y))).Take(12).OrderBy(x => x.Key);
            if (hits == null)
            {
                return;
            }
            if (hits.Count() == 0)
            {
                return;
            }
            int y_offset = (hits.Count() - 1) * 26;
            IEnumerator<KeyValuePair<string, MethodInfo>> enumerator = null;
            try
            {
                int maxWidth = 10;
                enumerator = hits.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    MethodInfo method = enumerator.Current.Value;
                    string category = method.DeclaringType.Name;
                    if (m_ProxyDict.ContainsKey(category))
                    {
                        try
                        {
                            string name = enumerator.Current.Key.Substring(enumerator.Current.Key.IndexOf('.') + 1);
                            SizeF siz = GH_FontServer.MeasureString(name, Font);
                            Rectangle nRegion = new Rectangle(0, y_offset, (int)Math.Ceiling(siz.Width + 50), 26);
                            GH_Hit new_hit = new GH_Hit(m_ProxyDict[category], name, method, nRegion);
                            m_hits.Add(new_hit);
                            y_offset -= 26;
                            maxWidth = Math.Max(maxWidth, nRegion.Width);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
                Width = maxWidth + 5;
            }
            finally
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                }
            }
            SelectedIndex = SelectedIndex;
        }


        private void txtSearch_LostFocus(object sender, System.EventArgs e)
        {
            FadeOut();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (keyCode == Keys.Up)
            {
                int prev_index = SelectedIndex;
                SelectedIndex++;
                if (SelectedIndex == prev_index)
                {
                    SelectedIndex = 0;
                }
                pnlHits.Refresh();
            }
            else if (keyCode == Keys.Down)
            {
                int prev_index2 = SelectedIndex;
                SelectedIndex--;
                if (SelectedIndex == prev_index2)
                {
                    SelectedIndex = 2147483647;
                }
                pnlHits.Refresh();
            }
            else if (keyCode == Keys.Return || keyCode == Keys.Return)
            {
                if (txtSearch.Text.Equals("Crash!", System.StringComparison.OrdinalIgnoreCase))
                {
                    throw new System.Exception("This is a debugging crash that was specifically triggered by the user.");
                }
                InsertComponent();
            }
            else
            {
                if (keyCode != Keys.Escape && keyCode != Keys.Cancel)
                {
                    return;
                }
                FadeOut();
            }
            e.Handled = true;
        }

        private void txtSearch_TextChanged(object sender, System.EventArgs e)
        {
            string text = txtSearch.Text;
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                SetDefaultMessage();
                return;
            }
            PopulateHitList(text);
            if (m_hits.Count == 0)
            {
                SetNoResultsMessage();
            }
            else
            {
                ClearMessage();
            }
        }

        public void SetDefaultMessage()
        {
            SetMessage("Enter a search keyword…");
        }

        public void SetNoResultsMessage()
        {
            SetMessage("No search results…");
        }

        public void ClearMessage()
        {
            SetMessage(null);
        }

        public void SetMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                m_message = null;
                pnlHits.Height = m_hits.Count * 26;
            }
            else
            {
                m_message = msg;
                SizeF siz = GH_FontServer.MeasureString(msg, Font);
                pnlHits.Height = System.Convert.ToInt32(siz.Height + 25f);
            }
            LayoutPopupDialog();
            pnlHits.Refresh();
        }

        private void pnlHits_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (string.IsNullOrEmpty(m_message))
            {
                PaintHits(e.Graphics);
            }
            else
            {
                PaintMessage(e.Graphics);
            }
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, pnlHits.Width - 1, pnlHits.Height + 2);
        }

        private void PaintMessage(Graphics g)
        {
            SolidBrush brush_text = new SolidBrush(Color.FromArgb(100, ForeColor));
            Font font_text = GH_FontServer.NewFont(Font, FontStyle.Italic);
            StringFormat format_text = new StringFormat();
            format_text.Alignment = StringAlignment.Center;
            format_text.LineAlignment = StringAlignment.Center;
            Rectangle rec = pnlHits.Bounds;
            rec.Inflate(-2, -2);
            g.TextRenderingHint = GH_TextRenderingConstants.GH_CrispText;
            g.DrawString(m_message, font_text, Brushes.Black, rec, format_text);
            brush_text.Dispose();
            font_text.Dispose();
            format_text.Dispose();
        }

        private void PaintHits(Graphics g)
        {
            g.Clear(BackColor);
            g.SmoothingMode = SmoothingMode.None;
            int arg_23_0 = 0;
            int num = m_hits.Count - 1;
            for (int i = arg_23_0; i <= num; i++)
            {
                if (i == SelectedIndex)
                {
                    Rectangle rec = m_hits[i].region;
                    rec.X -= rec.Width;
                    rec.Width *= 2;
                    GH_GraphicsUtil.DentHorizontal(g, rec);
                }
            }
            g.TextRenderingHint = GH_TextRenderingConstants.GH_CrispText;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            SolidBrush fillInactive = new SolidBrush(Color.FromArgb(150, ForeColor));
            SolidBrush fillActive = new SolidBrush(ForeColor);
            int arg_BB_0 = 0;
            int num2 = m_hits.Count - 1;
            for (int j = arg_BB_0; j <= num2; j++)
            {
                Rectangle iconRegion = new Rectangle(m_hits[j].region.X + 5, m_hits[j].region.Y + 1, 24, 24);
                if (m_hits[j].proxy != null && m_hits[j].proxy.Icon != null)
                {
                    g.DrawImage(m_hits[j].proxy.Icon, iconRegion);
                }
                GH_GraphicsUtil.RenderObjectOverlay(g, m_hits[j].proxy, iconRegion);
                Rectangle text_layout = m_hits[j].region;
                text_layout.X += 35;
                text_layout.Width -= 35;
                if (j == SelectedIndex)
                {
                    g.DrawString(m_hits[j].name, Font, fillActive, text_layout, GH_TextRenderingConstants.NearCenter);
                }
                else
                {
                    g.DrawString(m_hits[j].name, Font, fillInactive, text_layout, GH_TextRenderingConstants.NearCenter);
                }
            }
            fillActive.Dispose();
            fillInactive.Dispose();
        }
    }
}
