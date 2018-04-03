using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace ProAddins
{
    internal class ClearDefExprButton : Button
    {
        protected override void OnClick()
        {
            FeatureLayer layer = (FeatureLayer)MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
            if (layer == null) return;
            this.SetDefinitionQueryAsync(layer);
        }

        // <summary>
        // sets the definition query on a layer asynchronously
        // </summary>
        //
        protected Task SetDefinitionQueryAsync(FeatureLayer layer)
        {
            return QueuedTask.Run(() =>
            {
                layer.SetDefinitionQuery("");
            });
        }
    }
}
