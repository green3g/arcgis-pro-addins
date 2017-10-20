using System.Threading.Tasks;
using System.Diagnostics;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Core.Geometry;
using System.Collections.Generic;
using ArcGIS.Core.Data;
using System;
using ArcGIS.Desktop.Framework.Dialogs;


namespace ProAddins
{
    internal class BeaconButton : MapTool
    {
        const string LAYER_NAME = "Parcel Boundaries";//"CIMPATH=parcel_boundaries.xml";

        protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                e.Handled = true; //Handle the event args to get the call to the corresponding async method
        }

        protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
        {
            return QueuedTask.Run(() =>
            {
                IReadOnlyList<Layer> layerList = MapView.Active.Map.FindLayers(LAYER_NAME, true);
                if(layerList.Count == 0)
                {
                    MessageBox.Show("There must be a layer named 'Parcel Boundaries' in order to open Beacon");
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
                    string parcelid = Convert.ToString(feature["PARCELID"]);
                    if(parcelid != null)
                    {
                        // open a web browser
                        string url = string.Format("https://beacon.schneidercorp.com/Application.aspx?AppID=74&LayerID=590&PageTypeID=4&PageID=504&KeyValue={0}", parcelid);
                        Process.Start(url);
                    } else
                    {
                        MessageBox.Show("That parcel is missing a parcel id");
                    }
                }
            });
        }
    }
}
