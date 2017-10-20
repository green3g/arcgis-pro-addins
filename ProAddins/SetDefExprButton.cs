using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Windows;

namespace ProAddins
{
    // a button to set definition expressions on a layer to the selected feature ids
    // usage: 
    //      1. select features
    //      2. select layer in the table of contents
    //      3. click button
    internal class SetDefExprButton : Button
    {
        protected override void OnClick()
        {
            FeatureLayer layer = MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
            this.SetDefinitionQueryAsync(layer);
            
        }

        protected Task SetDefinitionQueryAsync(FeatureLayer layer)
        {
            return QueuedTask.Run(() =>
            {
                var selected = layer.GetSelection().GetObjectIDs();
                string expr = string.Join(",", selected);
                var idField = layer.GetTable().GetDefinition().GetObjectIDField();
                layer.SetDefinitionQuery($"{idField} in ({expr})");
            });
        }
    }
}
