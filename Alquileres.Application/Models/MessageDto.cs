using MimeKit;

namespace Alquileres.Application.Models;

public sealed class MessageDto
{
    public List<MailboxAddress> To { get; set; } = [];

    public List<MailboxAddress> Bcc { get; set; } = [];

    public string Subject { get; set; }

    public string Content { get; set; }

    public List<(string, string, byte[])> Attachments { get; set; }

    public MessageDto(IEnumerable<string> to, IEnumerable<string> bcc, string subject, string content, List<(string, string, byte[])> attachments)
    {
        To.AddRange(to.Select(x => new MailboxAddress(x, x)));
        Bcc.AddRange(bcc.Select(x => new MailboxAddress(x, x)));
        Subject = subject;
        Content = content;
        Attachments = attachments;
    }

    public MessageDto(IEnumerable<string> to, string subject, string content, List<(string, string, byte[])> attachments = null)
    {
        To.AddRange(to.Select(x => new MailboxAddress(x, x)));
        Subject = subject;
        Content = content;
        Attachments = attachments;
    }
}