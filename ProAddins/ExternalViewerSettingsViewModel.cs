using System.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;

namespace ProAddins
{
    internal class ExternalViewerSettingsViewModel : Page
    {
        #region Private Properties

        private string _originalParcelURL;
        private string _parcelURL;
        private string _originalParcelLayer;
        private string _parcelLayer;
        private string _parcelIDField;
        private string _originalParcelIDField;
        private bool _originalViewerEnabled;
        private bool _viewerEnabled;


        #endregion Private Properties 

        #region Accessors
        public string ParcelURL
        {
            get { return _parcelURL; }
            set
            {
                if (SetProperty(ref _parcelURL, value, () => ParcelURL))
                    base.IsModified = true;
            }
        }

        public string ParcelLayer
        {
            get { return _parcelLayer; }
            set
            {
                if (SetProperty(ref _parcelLayer, value, () => ParcelLayer))
                    base.IsModified = true;
            }
        }

        public string ParcelIDField
        {
            get { return _parcelIDField; }
            set
            {
                if (SetProperty(ref _parcelIDField, value, () => ParcelIDField))
                    base.IsModified = true;
            }
        }

        public bool ViewerEnabled
        {
            get { return _viewerEnabled; }
            set
            {
                if (SetProperty(ref _viewerEnabled, value, () => ViewerEnabled))
                    base.IsModified = true;
            }
        }

        private bool IsDirty()
        {
            if (_originalParcelURL != ParcelURL)
            {
                return true;
            }

            if(_originalParcelLayer != ParcelLayer)
            {
                return true;
            }

            if(_originalParcelIDField != ParcelIDField)
            {
                return true;
            }

            if (_originalViewerEnabled != ViewerEnabled)
            {
                return true;
            }

            return false;
        }

        #endregion Accessors


        #region Page Overrides
        /// <summary>
        /// Invoked when the OK or apply button on the property sheet has been clicked.
        /// </summary>
        /// <returns>A task that represents the work queued to execute in the ThreadPool.</returns>
        /// <remarks>This function is only called if the page has set its IsModified flag to true.</remarks>
        protected override Task CommitAsync()
        {
            if (IsDirty())
            {
                // save new settings

                Pro.settings.ParcelURL = ParcelURL;
                Pro.settings.ParcelLayer = ParcelLayer;
                Pro.settings.ParcelIDField = ParcelIDField;
                Pro.settings.ViewerEnabled = ViewerEnabled;

                Pro.settings.Save();
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Called when the page loads because to has become visible.
        /// </summary>
        /// <returns>A task that represents the work queued to execute in the ThreadPool.</returns>
        protected override Task InitializeAsync()
        {

            // assign values binding to controls 
            _parcelURL = Pro.settings.ParcelURL;
            _parcelLayer = Pro.settings.ParcelLayer;
            _parcelIDField = Pro.settings.ParcelIDField;
            _viewerEnabled = Pro.settings.ViewerEnabled;

            // keep track of original values
            _originalParcelURL = ParcelURL;
            _originalParcelLayer = ParcelLayer;
            _originalParcelIDField = ParcelIDField;
            _originalViewerEnabled = ViewerEnabled;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Called when the page is destroyed.
        /// </summary>
        protected override void Uninitialize()
        {
        }

        #endregion

    }
}
