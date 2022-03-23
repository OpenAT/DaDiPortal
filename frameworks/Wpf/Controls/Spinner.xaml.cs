using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Controls
{
    public partial class Spinner : UserControl
    {
        #region ctors

        static Spinner()
        {
            DiameterProperty = DependencyProperty.Register("Diameter", typeof(double), typeof(Spinner), new PropertyMetadata(12.0, OnDiameterPropertyChanged));
            RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(Spinner), new PropertyMetadata(6.0, null, OnCoerceRadius));
            InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(Spinner), new PropertyMetadata(2.0, null, OnCoerceInnerRadius));
            CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(Spinner), new PropertyMetadata(new Point(15.0, 15.0), null, OnCoerceCenter));
            Color2Property = DependencyProperty.Register("Color2", typeof(Color), typeof(Spinner), new PropertyMetadata(Colors.Transparent));
            Color1Property = DependencyProperty.Register("Color1", typeof(Color), typeof(Spinner), new PropertyMetadata(Colors.Green));
        }

        public Spinner()
        {
            InitializeComponent();
        }

        #endregion

        #region dependency props

        public static readonly DependencyProperty DiameterProperty;
        public static readonly DependencyProperty RadiusProperty;
        public static readonly DependencyProperty InnerRadiusProperty;
        public static readonly DependencyProperty CenterProperty;
        public static readonly DependencyProperty Color1Property;
        public static readonly DependencyProperty Color2Property;

        #endregion

        #region props

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set
            {
                if (value < 10)
                    value = 10;

                SetValue(DiameterProperty, value);
            }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public Color Color1
        {
            get { return (Color)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        public Color Color2
        {
            get { return (Color)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        #endregion

        #region event handlers

        private static void OnDiameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(CenterProperty);
            d.CoerceValue(RadiusProperty);
            d.CoerceValue(InnerRadiusProperty);
        }

        #endregion

        #region methods

        private static object OnCoerceRadius(DependencyObject d, object baseValue)
        {
            var control = (Spinner)d;
            double newRadius = (double)(control.GetValue(DiameterProperty)) / 2.0;
            return newRadius;
        }

        private static object OnCoerceInnerRadius(DependencyObject d, object baseValue)
        {
            var control = (Spinner)d;
            double newInnerRadius = (double)(control.GetValue(DiameterProperty)) / 2.5;
            return newInnerRadius;
        }

        private static object OnCoerceCenter(DependencyObject d, object baseValue)
        {
            var control = (Spinner)d;
            double newCenter = (double)(control.GetValue(DiameterProperty)) / 2.0f;
            return new Point(newCenter, newCenter);
        }

        #endregion
    }
}
