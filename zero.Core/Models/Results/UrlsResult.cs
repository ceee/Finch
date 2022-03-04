namespace zero.Models;

public class UrlsResult
{
  public string[] Urls { get; set; } = Array.Empty<string>();

  public string Domain { get; set; }

  public UrlsResult(string domain, params string[] urls)
  {
    Domain = domain;
    Urls = urls.Where(x => x.HasValue()).ToArray();
  }

  public UrlsResult(Uri domain, params string[] urls)
  {
    Domain = domain.Scheme + "://" + domain.Authority.TrimEnd("/");
    Urls = urls.Where(x => x.HasValue()).ToArray();
  }
}


public class PreviewUrlResult
{
  public string Url { get; set; }

  public PreviewUrlResult(string url)
  {
    Url = url;
  }
}