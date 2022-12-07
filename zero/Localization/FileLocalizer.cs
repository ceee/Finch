using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Options;

namespace zero.Localization;

public class FileLocalizer : Localizer
{
  private List<Translation> _fileList;
  private IWebHostEnvironment _env;


  public FileLocalizer(IWebHostEnvironment env, IOptionsMonitor<LocalizationOptions> options) : base()
  {
    _env = env;
    LoadIntoCache(options.CurrentValue.FilePath);
    options.OnChange((opts, _) => LoadIntoCache(opts.FilePath));
  }


  protected void LoadIntoCache(string filePath)
  {
    string path = Path.Combine(_env.ContentRootPath, filePath);

    if (!File.Exists(path))
    {
      _fileList = new();
    }
    else
    {
      Dictionary<string, string> texts = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path, Encoding.Latin1));
      _fileList = texts.Select(kvp => new Translation() { Key = kvp.Key, Value = kvp.Value }).ToList();
    }
  }


  protected override Translation LoadTranslation(string key)
  {
    //return new Translation()
    //{
    //  Value = "{@" + key + "}"
    //};

    return _fileList.FirstOrDefault(x => x.Key == key);
  }
}