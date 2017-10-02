using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace BH.UI.Alligator.MachineLearning
{
    public class AnimationTimer : EventArgs
    {
        private System.Windows.Threading.DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer();
        private double _simTimeOnTimerStart;
        private double _animationSpeed;
        private Stopwatch _realTime = new Stopwatch();

        public System.Windows.Threading.DispatcherTimer Timer { get { return _timer; } }

        public AnimationTimer()
        {
            _timer.Tick += new EventHandler(dispatcherTimer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 20); // 20 Frames/s
            _simTimeOnTimerStart = 0;
            _animationSpeed = 1;
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Start(double time = 0)
        {
            _simTimeOnTimerStart = time;
            _realTime.Restart();
            _timer.Start();
        }

        public void Reset()
        {
            _simTimeOnTimerStart = 0;
            _realTime.Reset();
        }

        public double Speed
        {
            set
            {
                Reset();
                _animationSpeed = value;
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //We use the realtime, just a counter would not be accurate if the is a lot of delays
            double newTime = _simTimeOnTimerStart + _animationSpeed * _realTime.Elapsed.TotalSeconds;
            //((SmartMoveAnimationGUI)WidgetInterface.GetWidget(WidgetController.WidName.SmartAnimation).UI).UpdateAnalysisTime(newTime);
            //Utility.gAnimateWindow.UpdateAnalysisTime(newTime);
        }


    }
}
