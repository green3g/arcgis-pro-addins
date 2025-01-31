using System.Threading.Tasks;
using System.Diagnostics;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Core.Geometry;
using System.Globalization;

namespace ProAddins
{
    internal class OpenStreetviewTool : MapTool
    {
        protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                e.Handled = true; //Handle the event args to get the call to the corresponding async method
        }

        protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
        {
            return QueuedTask.Run(() =>
            {
                // Convert the clicked point in client coordinates to the corresponding map coordinates.
                MapPoint mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
                
                // convert to lon/lat
                MapPoint coords = (MapPoint)GeometryEngine.Instance.Project(mapPoint, SpatialReferences.WGS84);

                // open a web browser
                CultureInfo culture = new CultureInfo("en-US");
                string url = string.Format("https://www.google.com/maps/@?api=1&map_action=pano&viewpoint={0}%2C{1}", coords.Y.ToString(culture), coords.X.ToString(culture));
                //string url = string.Format("http://maps.google.com/?cbll={0},{1}&cbp=12,90,0,0,5&layer=c", coords.Y.ToString(culture), coords.X.ToString(culture));
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            });
        }
    }
}
