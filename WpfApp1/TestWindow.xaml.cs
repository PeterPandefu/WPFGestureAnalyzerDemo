using GestureAnalyzer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window, INotifyPropertyChanged
    {
        private Dictionary<int, UIElement> movingEllipses = new Dictionary<int, UIElement>();
        private Point _StartPoint = new Point(0, 0);
        private Point _EndPoint = new Point(0, 0);
        private Test2Window _TestWindow;
        private bool isStart = false;
        private List<System.Drawing.Point> _SettingPoints = new();
        private List<System.Drawing.Point> _CurrentPoints = new();
        private PointPatternAnalyzer analyzer = new PointPatternAnalyzer();


        private PointPattern[] pointPatterns;

        public PointPattern[] PointPatterns
        {
            get { return pointPatterns; }
            set { SetProperty(ref pointPatterns, value); }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return;

            storage = value;
            this.OnPropertyChanged(propertyName);
        }

        #endregion

        public TestWindow()
        {
            InitializeComponent();
            _TestWindow = new Test2Window();
            PointPatterns = new PointPattern[] { new PointPattern(new List<List<System.Drawing.Point>>() { new List<System.Drawing.Point>()}) };
            analyzer.PointPatternSet = new List<PointsPatternSet>();
        }

        private void Canvas_TouchDown(object sender, TouchEventArgs e)
        {

            //Create an ellipse to draw at the new contact point
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 30;
            ellipse.Height = 30;
            ellipse.Stroke = Brushes.White;
            ellipse.Fill = Brushes.Green;

            //Position the ellipse at the contact point
            TouchPoint touchPoint = e.GetTouchPoint(canvas);
            Canvas.SetTop(ellipse, touchPoint.Bounds.Top);
            Canvas.SetLeft(ellipse, touchPoint.Bounds.Left);

            //Store the ellipse in the active collection
            movingEllipses[e.TouchDevice.Id] = ellipse;

            //Add the ellipse to the Canvas
            canvas.Children.Add(ellipse);

            _CurrentPoints.Clear();
        }

        private void canvas_TouchUp(object sender, TouchEventArgs e)
        {
            TouchPoint touchPoint = e.GetTouchPoint(canvas);

            //Remove the ellipse from the Canvas
            UIElement element = movingEllipses[e.TouchDevice.Id];
            canvas.Children.Remove(element);

            //Remove the ellipse from the tracking collection
            movingEllipses.Remove(e.TouchDevice.Id);
            toggleButton.IsChecked = false;

            if (!isStart&& _CurrentPoints.Count>20&& analyzer.PointPatternSet != null && analyzer.PointPatternSet.Count() > 0)
            {
                 var tmp = analyzer.GetPointPatternMatchResults(_CurrentPoints.ToArray());
                if (tmp.Count() > 0)
                {
                    if (tmp.First().Probability > 80)
                    {
                        MessageBox.Show("手势匹配成功!");
                    }
                    else
                    {
                        MessageBox.Show("手势匹配失败!");
                    }
                }
            }


            if (isStart)
            {
                PointPatterns[0].Points[0] = _SettingPoints.ToArray();
                OnPropertyChanged(nameof(PointPatterns));
                //加入配置
                PointsPatternSet pointsPatternSet = new PointsPatternSet(new Random().Next(1, 100000).ToString(), _SettingPoints.ToArray());
                analyzer.PointPatternSet = new List<PointsPatternSet>() { pointsPatternSet };
                isStart = false;
            }

           

            //对比手势

        }

        private void canvas_TouchMove(object sender, TouchEventArgs e)
        {
            //Get the ellipse that corresponds to the current contact point
            UIElement element = movingEllipses[e.TouchDevice.Id];

            //Move it to the new contact point
            TouchPoint touchPoint = e.GetTouchPoint(canvas);

            Canvas.SetTop(element, touchPoint.Bounds.Top);
            Canvas.SetLeft(element, touchPoint.Bounds.Left);

            if (isStart )
            {
                _SettingPoints.Add(new System.Drawing.Point((int)touchPoint.Bounds.Left, (int)touchPoint.Bounds.Top));
            }

            _CurrentPoints.Add(new System.Drawing.Point((int)touchPoint.Bounds.Left, (int)touchPoint.Bounds.Top));
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isStart = true;
            _SettingPoints.Clear();
        }
    }
}
