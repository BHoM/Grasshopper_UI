using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Windows.Forms;
using SmartChartingToolkit.SmartData;

namespace StadiaCrowdAnalytics_Alligator
{
    public class GraphingExtra : GH_Component
    {
        GraphWindow _graphWin;
        FramePlayer _framePlayer;
        bool _loaded;
        String _folderLocation;
        Grasshopper.Kernel.Special.GH_NumberSlider _slider;
        Grasshopper.Kernel.Special.GH_BooleanToggle _analysedFramesToggle;
        Grasshopper.Kernel.Special.GH_BooleanToggle _meshToggle;
        Grasshopper.Kernel.Special.GH_Panel _panel;

        //DataManager _dManager;
        //DataCollection _dCollection;

        SmartDataController _dController;
        SmartDataCollection _dCollection;
        Dictionary<int, double> _avgValues;

        VideoAnalyser _va;
        
        public GraphingExtra()
            : base("StadiaGraphWindow", "StadiaGraphWindow", "Display a graph window", "Alligator", "StadiaCrowdAnalytics")
        {
            _graphWin = new GraphWindow();
            _graphWin.PlayBtn.Click += Click_PlayBtn;
            _graphWin.ResetBtn.Click += Click_ResetBtn;
            _graphWin.TimeSlider.ValueChanged += Change_TimeSlider;
            _graphWin.LoadBtn.Click += Click_LoadBtn;
            _graphWin.ExportBtn.Click += Click_ExportBtn;
            _graphWin.DisplayAnalysedFramesBox.Checked += Check_DisplayAnalysedFrames;
            _graphWin.DisplayAnalysedFramesBox.Unchecked += Check_DisplayAnalysedFrames;
            _graphWin.DisplayMeshBox.Checked += Check_DisplayMesh;
            _graphWin.DisplayMeshBox.Unchecked += Check_DisplayMesh;

            _loaded = false;

            /*_dManager = new DataManager();
            _dCollection = new DataCollection();
            _dCollection.CollectionColour = System.Windows.Media.Colors.Black;
            _dCollection.CollectionGraphType = DataCollection.GraphType.Line;*/

            _dController = new SmartDataController();
            _dCollection = new SmartDataCollection();
            _dCollection.Colour = System.Windows.Media.Colors.Black;
            _avgValues = new Dictionary<int, double>();

            

            _va = null;
        }

        public override void AddedToDocument(GH_Document document)
        {
            _slider = new Grasshopper.Kernel.Special.GH_NumberSlider();
            _slider.Slider.DecimalPlaces = 0;
            _slider.Slider.Minimum = 0;

            Grasshopper.Instances.ActiveCanvas.Document.AddObject(_slider, false);

            _analysedFramesToggle = new Grasshopper.Kernel.Special.GH_BooleanToggle();
            _analysedFramesToggle.Value = false;
            Grasshopper.Instances.ActiveCanvas.Document.AddObject(_analysedFramesToggle, false);

            _meshToggle = new Grasshopper.Kernel.Special.GH_BooleanToggle();
            _meshToggle.Value = false;
            Grasshopper.Instances.ActiveCanvas.Document.AddObject(_meshToggle, false);

            _panel = new Grasshopper.Kernel.Special.GH_Panel();
            Grasshopper.Instances.ActiveCanvas.Document.AddObject(_panel, false);

            base.AddedToDocument(document);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("755CBABA-3504-45D6-8BA1-A8C921EEBC70"); }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Start", "Start", "Start graph", GH_ParamAccess.item, true);
            pManager.AddGenericParameter("AnalysedData", "AnalysedData", "Data that was analysed to build a mesh with", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface to paint on", GH_ParamAccess.item);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool start = GHE.DataUtils.GetData<bool>(DA, 0);
            _va = (VideoAnalyser)GHE.DataUtils.GetData<object>(DA, 1);
            Grasshopper.Kernel.Types.GH_Surface srf = GHE.DataUtils.GetData<Grasshopper.Kernel.Types.GH_Surface>(DA, 2);

            if (start)
                _graphWin.Show();
            else
                _graphWin.Hide();

            if (_va == null || srf == null) return;

            FramePlayer tmp = new FramePlayer();
            _framePlayer = (FramePlayer)Grasshopper.Instances.ActiveCanvas.Document.Objects.Where(x => x.ComponentGuid == tmp.ComponentGuid).FirstOrDefault();

            if (_framePlayer == null)
            {
                //Frameplayer is not on the canvas yet - add it
                _framePlayer = new FramePlayer();
                Grasshopper.Instances.ActiveCanvas.Document.AddObject(_framePlayer, false);
            }

            if(_framePlayer.Params.Input[0].SourceCount == 0)
                _framePlayer.Params.Input[0].AddSource(_panel);
            if(_framePlayer.Params.Input[1].SourceCount == 0)
                _framePlayer.Params.Input[1].AddSource(_analysedFramesToggle);
            if(_framePlayer.Params.Input[2].SourceCount == 0)
                _framePlayer.Params.Input[2].AddSource(_slider);
            if(_framePlayer.Params.Input[3].SourceCount == 0)
                _framePlayer.Params.Input[3].AddSource(this.Params.Input[2].Sources[0]);
            if(_framePlayer.Params.Input[4].SourceCount == 0)
                _framePlayer.Params.Input[4].AddSource(this.Params.Input[1].Sources[0]); //Add video analysed data
            if(_framePlayer.Params.Input[5].SourceCount == 0)
                _framePlayer.Params.Input[5].AddSource(_meshToggle);

            _graphWin.SetMaxFrames(_va.MaxFrame);
            _slider.Slider.Maximum = _va.MaxFrame;

            for (int x = 1; x < _va.MaxFrame; x++)
            {
                if (!_avgValues.ContainsKey(x))
                    _avgValues.Add(x, _va.AverageResult(x));
            }

            /*_dCollection.Clear();
            for (int x = 1; x < _va.MaxFrame; x++)
                _dCollection.AddItem(new DataItem(x.ToString(), _avgValues[x]));

            _dManager.ClearCollections();
            _dManager.AddCollection(_dCollection);

            _graphWin.GraphChart.DataController = _dManager;*/

            _dCollection.ClearData();
            for (int x = 1; x < _va.MaxFrame; x++)
                _dCollection.AddData(new SmartDataItem(x.ToString(), _avgValues[x]));

            _dController.ClearCollections();
            _dController.AddCollection(_dCollection);
            _graphWin.SmartChart.SetDataController(_dController);

                if (start)
                    _graphWin.Show();
                else
                    _graphWin.Hide();
        }

        private void Click_PlayBtn(object sender, EventArgs e)
        {
            if (!_loaded) return;

            _graphWin.Play();
        }

        private void Click_ResetBtn(object sender, EventArgs e)
        {
            _graphWin.Reset();
        }

        private void Change_TimeSlider(object sender, EventArgs e)
        {
            int timestep = (int)_graphWin.TimeSlider.Value;

            _slider.SetSliderValue((decimal)timestep);
            _slider.ExpireSolution(true);

            double graphFollow = timestep / (double)_va.MaxFrame;
            double xPos = ((double)(_graphWin.GraphFollowerBottom.X2 - _graphWin.GraphFollowerBottom.X1)) * graphFollow;
            xPos += _graphWin.GraphFollowerBottom.X1;
            _graphWin.GraphFollower.X1 = xPos;
            _graphWin.GraphFollower.X2 = xPos;

            /*if (!_avgValues.ContainsKey(timestep))
            {
                _avgValues.Add(timestep, _va.AverageResult(timestep));
            }

            _dCollection.Clear();
            for (int x = 1; x < timestep; x++)
                _dCollection.AddItem(new DataItem(x.ToString(), _avgValues[x]));

            _dManager.ClearCollections();
            _dManager.AddCollection(_dCollection);*/
        }

        private void Click_LoadBtn(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    _folderLocation = fbd.SelectedPath;
                    _loaded = true;

                    //Change panel text and connect to frame player
                    _panel.SetUserText(_folderLocation);
                    //_panel.ExpirePreview(true);

                    //Grasshopper.Instances.ActiveCanvas.Refresh();
                }
            }
        }

        private void Click_ExportBtn(object sender, EventArgs e)
        {
            
        }

        private void Check_DisplayAnalysedFrames(object sender, EventArgs e)
        {
            if (((System.Windows.Controls.CheckBox)sender).IsChecked == true)
                _analysedFramesToggle.Value = true;
            else
                _analysedFramesToggle.Value = false;

            _analysedFramesToggle.ExpireSolution(true);
        }

        private void Check_DisplayMesh(object sender, EventArgs e)
        {
            if (((System.Windows.Controls.CheckBox)sender).IsChecked == true)
                _meshToggle.Value = true;
            else
                _meshToggle.Value = false;

            _meshToggle.ExpireSolution(true);
        }
    }
}
