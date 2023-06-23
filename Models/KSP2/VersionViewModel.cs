using ReactiveUI;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ToucanUI.Models.KSP2
{
    public class VersionViewModel : ViewModelBase
    {
        // =====================
        // VARIABLES
        // =====================
        public Version VersionObject { get; set; }
        private readonly ModViewModel _modViewModel;

        // If version is selected in the dropdown
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    this.RaiseAndSetIfChanged(ref _isSelected, value);

                    if (_isSelected)
                    {
                        // Call OnVersionSelected on the ModViewModel instance
                        _modViewModel.OnVersionSelected(this);
                    }
                }
            }
        }


        // Has Version download size been fetched
        private bool _isDownloadSizeFetched;
        public bool IsDownloadSizeFetched
        {
            get => _isDownloadSizeFetched;
            set => this.RaiseAndSetIfChanged(ref _isDownloadSizeFetched, value);
        }

        private long? _downloadSize;
        public long? DownloadSize
        {
            get => _downloadSize;
            set => this.RaiseAndSetIfChanged(ref _downloadSize, value);
        }

        private string _downloadSizeDisplay;
        public string DownloadSizeDisplay
        {
            get => _downloadSizeDisplay;
            set => this.RaiseAndSetIfChanged(ref _downloadSizeDisplay, value);
        }



        // =====================
        // CONSTRUCTORS
        // =====================
        public VersionViewModel(Version version, ModViewModel modViewModel)
        {
            _modViewModel = modViewModel; // Store the ModViewModel instance
            VersionObject = version;
            IsSelected = false;

            // Observe changes to the DownloadSize property
            this.WhenAnyValue(x => x.DownloadSize)
                .Select(downloadSize => GetDownloadSizeDisplay(downloadSize))
                .Subscribe(downloadSizeDisplay => DownloadSizeDisplay = downloadSizeDisplay);
            _modViewModel = modViewModel;
        }

        // =====================
        // METHODS
        // =====================

        // Gets the download size of a mod version
        public async Task GetDownloadSizeAsync()
        {
            if (DownloadSize != null || IsDownloadSizeFetched) return;  // Skip if the download size is already fetched

            using var httpClient = new HttpClient();
            var downloadUrl = VersionObject.DownloadPath;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));

            try
            {
                //Debug.WriteLine($"Getting download size for {VersionObject.FriendlyVersion}");
                var request = new HttpRequestMessage(HttpMethod.Head, downloadUrl);
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var contentLength = response.Content.Headers.ContentLength;
                    DownloadSize = contentLength;
                }
                else
                {
                    DownloadSize = null;
                }
            }
            catch
            {
                DownloadSize = null;
            }

            IsDownloadSizeFetched = true;
        }

        private string GetDownloadSizeDisplay(long? downloadSize)
        {
            if (downloadSize != null)
            {
                double sizeInBytes = (double)downloadSize;
                string[] sizeSuffixes = { "B", "KB", "MB", "GB", "TB" };
                int suffixIndex = 0;

                while (sizeInBytes >= 1024 && suffixIndex < sizeSuffixes.Length - 1)
                {
                    sizeInBytes /= 1024;
                    suffixIndex++;
                }

                sizeInBytes = Math.Round(sizeInBytes); // Round the sizeInBytes to the nearest whole number

                return $"{sizeInBytes} {sizeSuffixes[suffixIndex]}";
            }
            else
            {
                return "...";
            }
        }

    }
}
