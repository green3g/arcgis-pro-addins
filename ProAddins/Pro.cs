using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ProEvergreen;
using Octokit;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Threading.Tasks;

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

        /// <summary>
        /// Called when the addin initializes to check for updates and download the latest
        /// </summary>
        /// <returns>Task</returns>
        public static Task RunAsyncUpdateCheck()
        {
            return QueuedTask.Run(async () => { 

                // check current release and update it if there's a newer version
                var evergreen = new Evergreen("roemhildtg", "arcgis-pro-addins");
                VersionInformation currentVersion = evergreen.GetCurrentAddInVersion();
                Release latestVersion = await evergreen.GetLatestReleaseFromGithub();
                if (!evergreen.IsCurrent(currentVersion.AddInVersion, latestVersion))
                {
                    await evergreen.Update(latestVersion);
                    var notify = new ArcGIS.Desktop.Framework.Notification();
                    notify.Title = "Addin Update";
                    notify.Message = string.Format("Your pro-addins have been updated to version {0}. Please restart Pro to complete the update.", latestVersion.TagName);
                    FrameworkApplication.AddNotification(notify);
                }

            });
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

        protected override bool Initialize()
        {
            RunAsyncUpdateCheck();
            return true;
        }

        #endregion Overrides

    }
}
