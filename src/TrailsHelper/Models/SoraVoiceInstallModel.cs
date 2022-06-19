using Octokit;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Readers;


namespace TrailsHelper.Models
{
    internal class SoraVoiceInstallModel : IDisposable
    {
        private bool disposedValue;

        public SoraVoiceInstallModel(string modPrefix, string gamePath)
        {
            this.Prefix = modPrefix;
            this.GamePath = gamePath;
            this.GithubClient = new GitHubClient(new ProductHeaderValue("trailshelper"));
            var handler = new HttpClientHandler() { AllowAutoRedirect = true };
            var ph = new ProgressMessageHandler(handler);

            ph.HttpReceiveProgress += (_, args) => this.ProgressChangedEvent?.Invoke(this, ((double)args.BytesTransferred / args.TotalBytes) * 100 ?? 0);

            this.HttpClient = new HttpClient(ph);
        }

        public string Prefix { get; }
        public string GamePath { get; }
        public GitHubClient GithubClient { get; }
        public HttpClient HttpClient { get; }

        public event EventHandler<double>? ProgressChangedEvent;
        
        public async Task<Stream> DownloadLatestMod()
        {
            var releases = await this.GithubClient.Repository.Release.GetLatest("ZhenjianYang", "SoraVoice");
            var asset = releases.Assets.First(s => s.Name.StartsWith("SoraVoiceLite"));
            var stream = await this.HttpClient.GetStreamAsync(asset.BrowserDownloadUrl);
            var outStream = new MemoryStream();
            await stream.CopyToAsync(outStream);
            await outStream.FlushAsync();
            return outStream;
        }

        public async Task<Stream> DownloadLatestScripts()
        {
            var releases = await this.GithubClient.Repository.Release.GetLatest("ZhenjianYang", "SoraVoiceScripts");
            var asset = releases.Assets.First(s => s.Name.StartsWith(this.Prefix));
            var stream = await this.HttpClient.GetStreamAsync(asset.BrowserDownloadUrl);
            var outStream = new MemoryStream();
            await stream.CopyToAsync(outStream);
            await outStream.FlushAsync();
            return outStream;
        }

        public async Task<Stream> DownloadDialogueVoiceArchive()
        {
           
            //var releases = await this.GithubClient.Repository.Release.GetLatest("ZhenjianYang", "SoraVoiceScripts");
            //var asset = releases.Assets.First(s => s.Name.StartsWith(this.Prefix));
            //var stream = await this.HttpClient.GetStreamAsync(asset.BrowserDownloadUrl);
            var outStream = new MemoryStream();
            //await stream.CopyToAsync(outStream);
            //await outStream.FlushAsync();
            return outStream;
        }


        public void ExtractMod(Stream modStream)
        {
            var archive = ArchiveFactory.Open(modStream);
            using var reader = archive.ExtractAllEntries();
            reader.EntryExtractionProgress += (_, args) =>
            {
                this.ProgressChangedEvent?.Invoke(this, args.ReaderProgress?.PercentageReadExact ?? 0);
            };

            reader.WriteAllToDirectory(this.GamePath, new()
            {
                ExtractFullPath = true,
                Overwrite = true,
            });
        }

        public void ExtractScript(Stream modStream)
        {
            var archive = ArchiveFactory.Open(modStream);
            using var reader = archive.ExtractAllEntries();
            reader.EntryExtractionProgress += (_, args) =>
            {
                this.ProgressChangedEvent?.Invoke(this, args.ReaderProgress?.PercentageReadExact ?? 0);
            };
            
            reader.WriteAllToDirectory(Path.Combine(this.GamePath, "voice"), new()
            {
                ExtractFullPath = true,
                Overwrite = true,
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.HttpClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SoraVoiceInstallModel()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
