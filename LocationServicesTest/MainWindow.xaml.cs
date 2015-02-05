using System;
using System.Device.Location;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;

namespace LocationServicesTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GeoCoordinateWatcher _watcher;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

           
                _watcher = new GeoCoordinateWatcher();
                _watcher.PositionChanged += PositionChanged;
                _watcher.StatusChanged += StatusChanged;
                _watcher.Start(false);

                if (!_watcher.Position.Location.IsUnknown)
                    UpdatePosition(new Location { Latitude = _watcher.Position.Location.Latitude, Longitude = _watcher.Position.Location.Longitude });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                StatusEllipse.Fill = new SolidColorBrush{Color = Colors.LimeGreen};
            }
            else
            {
                StatusEllipse.Fill = new SolidColorBrush { Color = Colors.Red };
            }
        }

        void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            try
            {
                Latitude.Text = e.Position.Location.Latitude.ToString(CultureInfo.InvariantCulture);
                Longitude.Text = e.Position.Location.Longitude.ToString(CultureInfo.InvariantCulture);
                VertialAccuracy.Text = e.Position.Location.VerticalAccuracy.ToString(CultureInfo.InvariantCulture);
                HorizontalAccuracy.Text = e.Position.Location.HorizontalAccuracy.ToString(CultureInfo.InvariantCulture);

                UpdatePosition(new Location { Latitude = e.Position.Location.Latitude, Longitude = e.Position.Location.Longitude });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void UpdatePosition(Location location)
        {
            Console.WriteLine(@"Latitude: {0}, Longitude {1}", location.Latitude, location.Longitude);
           
            var point = ProjectionHelper.ToWebMercator(location);
            GpsLayer.ClearGraphics();

            GpsLayer.Graphics.Add(new Graphic { Geometry = point, Symbol = new SimpleMarkerSymbol { Color = new SolidColorBrush { Color = Colors.Red}, Size = 10}});
        }
    }
}
