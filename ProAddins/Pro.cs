using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Desktop.Framework.Dialogs;
using System.Windows;

namespace ProAddins
{
    internal class Pro : Module
    {
        private static Pro _this = null;
        private static Settings _settings = Settings.Default;

        /// <summary>
        /// Retrieve the singleton instance to this module here
        /// </summary>
        public static Pro Current
        {
            get
            {
                return _this ?? (_this = (Pro)FrameworkApplication.FindModule("ProAddins_Module"));
            }
        }

        public static Settings settings
        {
            get { return _settings; }
        }

      

        #region Overrides
        /// <summary>
        /// Called by Framework when ArcGIS Pro is closing
        /// </summary>
        /// <returns>False to prevent Pro from closing, otherwise True</returns>
        protected override bool CanUnload()
        {
            //TODO - add your business logic
            //return false to ~cancel~ Application close
            return true;
        }

        protected override bool Initialize() //Called when the Module is initialized.      
        {
            ProjectOpenedEvent.Subscribe(OnProjectOpened); //subscribe to Project opened event          
            return base.Initialize();
        }

        private void OnProjectOpened(ProjectEventArgs obj) //Project Opened event handler      
        {
            if (Pro.settings.ViewerEnabled)
            {
                FrameworkApplication.State.Activate("viewer_state");
            } else
            {
                FrameworkApplication.State.Deactivate("viewer_state");
            }
        }

        protected override void Uninitialize() //unsubscribe to the project opened event      
        {
            ProjectOpenedEvent.Unsubscribe(OnProjectOpened); //unsubscribe          
            return;
        }


        #endregion Overrides

  
    }
}
