public record Message
{
    public string? Token { get; init; }

    public int? BatchNo { get; set; }

    public string? CustomerCode { get; set; }

    public string? Action { get; init; }
}