using System.Threading.Tasks;
using System.Diagnostics;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Collections.Generic;
using ArcGIS.Core.Data;
using System;
using ArcGIS.Desktop.Framework.Dialogs;


namespace ProAddins
{
    internal class ExternalViewerButton : MapTool
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
                IReadOnlyList<Layer> layerList = MapView.Active.Map.FindLayers(Pro.settings.ParcelLayer, true);
                if(layerList.Count == 0)
                {
                    MessageBox.Show(string.Format("There must be a layer named '{0}' in order to open ExternalViewer. This can be configured in the Options", Pro.settings.ParcelLayer));
                    return;
                }

                FeatureLayer parcel = (FeatureLayer)layerList[0];

                SpatialQueryFilter filter = new SpatialQueryFilter
                {
                    FilterGeometry = MapView.Active.ClientToMap(e.ClientPoint),
                    SpatialRelationship = SpatialRelationship.Intersects
                };

                RowCursor parcelCursor = parcel.Search(filter);
                if (parcelCursor.MoveNext())
                {
                    Row feature = parcelCursor.Current;
                    string parcelid = Convert.ToString(feature[Pro.settings.ParcelIDField]);
                    if(parcelid != null)
                    {
                        // open a web browser
                        string url = string.Format(Pro.settings.ParcelURL, parcelid);
                        Process.Start(url);
                    } else
                    {
                        MessageBox.Show("That parcel is missing a parcel id, or you have misconfigured the parcelid field in the settings.");
                    }
                }
            });
        }
    }
}
