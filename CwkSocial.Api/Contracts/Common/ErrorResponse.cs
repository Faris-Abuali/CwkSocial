namespace CwkSocial.Api.Contracts.Common;

public class ErrorResponse
{
    public int Code { get; set; }
    public string Phrase { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = [];
    public DateTime Timestamp { get; set; }
}
