// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
//
// namespace Mixtape.Media.ImageSharp;
//
// public class RemoteImageCache : IRemoteImageCache
// {
//   protected IBoldCache Cache { get; set; }
//
//   protected IWebRootFileSystem FileSystem { get; set; }
//
//   protected ILogger<RemoteImageCache> Logger { get; set; }
//
//   protected ImagingRemoteCacheOptions Options { get; set; }
//
//   public const string CACHE_PREFIX = "remote:images:";
//
//
//   public RemoteImageCache(ILogger<RemoteImageCache> logger, IOptionsMonitor<ImagingOptions> monitor, IBoldCache cache, IWebRootFileSystem fileSystem)
//   {
//     Cache = cache;
//     Logger = logger;
//     FileSystem = fileSystem;
//     Options = monitor.CurrentValue.RemoteCache;
//     monitor.OnChange(opts => Options = opts.RemoteCache);
//   }
//
//
//   /// <inheritdoc />
//   public async Task<Media> Resolve(string url)
//   {
//     if (url.IsNullOrWhiteSpace())
//     {
//       return null;
//     }
//
//     url = url.Replace("hqdefault.jpg", "maxresdefault.jpg");
//
//     if (!Options.Enabled)
//     {
//       Logger.LogWarning("Tried to call remote image cache although the cache is disabled (url {url}", url);
//       return null;
//     }
//
//     string key = CACHE_PREFIX + url;
//
//     // try to find cached version first
//     Media media = Cache.Get<Media>(key);
//
//     if (media != null)
//     {
//       return media;
//     }
//
//     // download and store in filesystem
//     try
//     {
//       using Stream fileStream = await DownloadFile(url);
//       string fileName = url.Split('/').LastOrDefault();
//
//       string hash = Guid.NewGuid().ToString();
//       string path = "/uploads/" + hash + "/" + fileName;
//
//       await FileSystem.CreateFile(path, fileStream);
//
//       media = new()
//       {
//         Source = path,
//         RemotePath = url
//       };
//     }
//     catch (Exception ex)
//     {
//       Logger.LogError(ex, "Could not cache remote file {url}", url);
//       return null;
//     }
//
//     // cache file for one day
//     if (media != null)
//     {
//       Cache.Set(key, media, TimeSpan.FromDays(1));
//     }
//
//     return media;
//   }
//
//
//   /// <summary>
//   /// Downloads a remote file
//   /// </summary>
//   protected async Task<Stream> DownloadFile(string url, CancellationToken token = default)
//   {
//     using HttpClient http = new();
//     return await http.GetStreamAsync(url, token);
//   }
// }
//
//
// public interface IRemoteImageCache
// {
//   /// <summary>
//   /// Resolves or caches an external media file
//   /// </summary>
//   Task<Media> Resolve(string url);
// }