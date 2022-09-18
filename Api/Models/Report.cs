public class Report : BaseEntity
{
    public long MessageId { get; set; }
    public long ReporterId { get; set; }

    public long ReportMessageId { get; set; }
    public long ChatId { get; set; }
}
