namespace Shared.Common
{
    public class DetailError
    {
        public string Field { get; set; } = string.Empty;
        public string? Value { get; set; }
        public string MessageId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
