using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Test2Window.xaml 的交互逻辑
    /// </summary>
    public partial class Test2Window : Window
    {
        public Test2Window()
        {
            InitializeComponent();
        }

        private void image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            // Set the container (used for coordinates.)
            e.ManipulationContainer = canvas;

            // Choose what manipulations to allow.
            e.Mode = ManipulationModes.All;
        }

        private void image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // Get the image that's being manipulated.            
            FrameworkElement element = (FrameworkElement)e.Source;

            // Use the matrix to manipulate the element.
            Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;

            var deltaManipulation = e.DeltaManipulation;
            // Find the old center, and apply the old manipulations.
            Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
            center = matrix.Transform(center);

            // Apply zoom manipulations.
            matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);

            // Apply rotation manipulations.
            matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);

            // Apply panning.
            matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);

            // Set the final matrix.
            ((MatrixTransform)element.RenderTransform).Matrix = matrix;
        }

        private void canvas_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            //If the object is moving,decrease its speed by
            //10 inches per second every second
            //deceleration=10 inches * 96 units per inch /(1000 milliseconds)^2
            e.TranslationBehavior = new InertiaTranslationBehavior();
            e.TranslationBehavior.InitialVelocity = e.InitialVelocities.LinearVelocity;
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            //Decrease the speed of zooming by 0.1 inches per second every second.
            //deceleration=0.1 inches * 96 units per inch/(1000 milliseconds)^2
            e.ExpansionBehavior = new InertiaExpansionBehavior();
            e.ExpansionBehavior.InitialVelocity = e.InitialVelocities.ExpansionVelocity;
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            //Decrease the rotation rate by 2 rotations per second every second.
            //deceleration=2*36 degress /(1000 milliseconds)^2

            e.RotationBehavior = new InertiaRotationBehavior();
            e.RotationBehavior.InitialVelocity = e.InitialVelocities.AngularVelocity;
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);
        }
    }
}
