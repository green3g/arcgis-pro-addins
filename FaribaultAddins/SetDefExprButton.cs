using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace FaribaultAddins
{
    internal class SetDefExprButton : Button
    {
        protected override void OnClick()
        {
            FeatureLayer layer = (FeatureLayer)MapView.Active.GetSelectedLayers().OfType<FeatureLayer>().FirstOrDefault();
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
