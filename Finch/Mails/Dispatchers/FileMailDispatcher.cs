//using System.IO;
//using System.Text;

//namespace Finch.Mails;

///// <summary>
///// Default implementation of an IMailSender which sends the mail a flat file
///// and therefore not using the SMTP channel.
///// Implementing real mail sending is up to the consuming application.
///// </summary>
//public class FileMailDispatcher : IMailDispatcher
//{
//  protected Queue<Mail> Queue { get; private set; } = new Queue<Mail>();

//  protected IPaths PathResolver { get; private set; }

//  string MailDirectory;


//  public FileMailDispatcher(IPaths pathResolver)
//  {
//    PathResolver = pathResolver;
//    MailDirectory = "mails";
//  }


//  /// <inheritdoc />
//  public void Enqueue(Mail message)
//  {
//    Queue.Enqueue(message);
//  }


//  /// <inheritdoc />
//  public async Task Send(CancellationToken token = default)
//  {
//    if (Queue.Count < 1)
//    {
//      return;
//    }

//    string folder = PathResolver.Map(MailDirectory);
//    PathResolver.Create(folder);

//    while (Queue.Count > 0)
//    {
//      Mail message = Queue.Dequeue();
//      string content = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
//      {
//        Formatting = Formatting.Indented,
//        TypeNameHandling = TypeNameHandling.None,
//        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//      });

//      string filename = Safenames.File(DateTimeOffset.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + message.To[0].Address + ".txt");
//      string path = PathResolver.Map(folder, filename);

//      await File.WriteAllTextAsync(path, content, token);
//    }
//  }


//  /// <inheritdoc />
//  public void Dispose() { }


//  /// <summary>
//  /// Creats the file content from a mail message
//  /// </summary>
//  string CreateContent(Mail message)
//  {
//    StringBuilder text = new();
//    text.AppendLine("To: " + message.To);
//    text.AppendLine("CC: " + message.CC);
//    text.AppendLine("Bcc: " + message.Bcc);
//    text.AppendLine("From: " + message.From);
//    text.AppendLine("Subject: " + message.Subject);
//    text.Append("Date: " + DateTimeOffset.UtcNow.ToString());

//    text.AppendLine();
//    text.AppendLine("=============== BODY ===============");
//    text.AppendLine();

//    text.AppendLine(message.Body);

//    return text.ToString();
//  }
//}
