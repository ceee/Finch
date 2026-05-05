// using System.IO;
// using System.Security.Cryptography;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc.ViewFeatures;
// using Microsoft.AspNetCore.WebUtilities;
// using Microsoft.Extensions.Caching.Memory;
// using Microsoft.Extensions.FileProviders;
//
// namespace Mixtape.FileStorage;
//
// /// <summary>
// /// Provides version hash for a specified file.
// /// </summary>
// internal sealed class ManifestFileVersionProvider : IFileVersionProvider
// {
//   private const string VersionKey = "v";
//
//   public ManifestFileVersionProvider(IWebHostEnvironment hostingEnvironment)
//   {
//     ArgumentNullException.ThrowIfNull(hostingEnvironment);
//
//     FileProvider = hostingEnvironment.WebRootFileProvider;
//     Cache = null; //cacheProvider.Cache;
//   }
//
//   public IFileProvider FileProvider { get; }
//
//   public IMemoryCache Cache { get; }
//
//
//   public string AddFileVersionToPath(PathString requestPathBase, string path)
//   {
//     ArgumentNullException.ThrowIfNull(path);
//
//     string resolvedPath = path;
//
//     int queryStringOrFragmentStartIndex = path.AsSpan().IndexOfAny('?', '#');
//     if (queryStringOrFragmentStartIndex != -1)
//     {
//       resolvedPath = path[..queryStringOrFragmentStartIndex];
//     }
//
//     if (Uri.TryCreate(resolvedPath, UriKind.Absolute, out Uri uri) && !uri.IsFile)
//     {
//       // Don't append version if the path is absolute.
//       return path;
//     }
//
//     if (Cache.TryGetValue<string>(path, out string value) && value is not null)
//     {
//       return value;
//     }
//
//     MemoryCacheEntryOptions cacheEntryOptions = new();
//     cacheEntryOptions.AddExpirationToken(FileProvider.Watch(resolvedPath));
//     IFileInfo fileInfo = FileProvider.GetFileInfo(resolvedPath);
//
//     if (!fileInfo.Exists &&
//         requestPathBase.HasValue &&
//         resolvedPath.StartsWith(requestPathBase.Value, StringComparison.OrdinalIgnoreCase))
//     {
//       string requestPathBaseRelativePath = resolvedPath.Substring(requestPathBase.Value.Length);
//       cacheEntryOptions.AddExpirationToken(FileProvider.Watch(requestPathBaseRelativePath));
//       fileInfo = FileProvider.GetFileInfo(requestPathBaseRelativePath);
//     }
//
//     if (fileInfo.Exists)
//     {
//       value = QueryHelpers.AddQueryString(path, VersionKey, GetHashForFile(fileInfo));
//     }
//     else
//     {
//       // if the file is not in the current server.
//       value = path;
//     }
//
//     cacheEntryOptions.SetSize(value.Length * sizeof(char));
//     Cache.Set(path, value, cacheEntryOptions);
//     return value;
//   }
//
//   private static string GetHashForFile(IFileInfo fileInfo)
//   {
//     using (Stream readStream = fileInfo.CreateReadStream())
//     {
//       byte[] hash = SHA256.HashData(readStream);
//       return WebEncoders.Base64UrlEncode(hash);
//     }
//   }
// }