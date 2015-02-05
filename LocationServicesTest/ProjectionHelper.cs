using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Client.Geometry;

namespace LocationServicesTest
{
    public class ProjectionHelper
    {
        public static MapPoint ToGeographic(MapPoint point)
        {
            if (Math.Abs(point.X) < 180 && Math.Abs(point.Y) < 90)
                return null;

            if ((Math.Abs(point.X) > 20037508.3427892) || (Math.Abs(point.Y) > 20037508.3427892))
                return null;

            double x = point.X;
            double y = point.Y;
            double num3 = x / 6378137.0;
            double num4 = num3 * 57.295779513082323;
            double num5 = Math.Floor((double)((num4 + 180.0) / 360.0));
            double num6 = num4 - (num5 * 360.0);
            double num7 = 1.5707963267948966 - (2.0 * Math.Atan(Math.Exp((-1.0 * y) / 6378137.0)));
            point.X = num6;
            point.Y = num7 * 57.295779513082323;

            return new MapPoint(point.X, point.Y, new SpatialReference(4326));
        }

        public static MapPoint ToWebMercator(MapPoint point)
        {
            if ((Math.Abs(point.X) > 180 || Math.Abs(point.Y) > 90))
                return null;

            double num = point.X * 0.017453292519943295;
            double x = 6378137.0 * num;
            double a = point.Y * 0.017453292519943295;

            point.X = x;
            point.Y = 3189068.5 * Math.Log((1.0 + Math.Sin(a)) / (1.0 - Math.Sin(a)));

            return new MapPoint(point.X, point.Y, new SpatialReference(3857));
        }


        public static MapPoint ToWebMercator(Location location)
        {
            return ToWebMercator(new MapPoint(location.Longitude, location.Latitude, new SpatialReference(4326)));
        }

        public static MapPoint ToGeographic(Location location)
        {
            return ToWebMercator(new MapPoint(location.Longitude, location.Latitude, new SpatialReference(3857)));
        }
    }
}
