public record PipelineState(bool Success, string[] AllErrors)
{
    public static PipelineState Ok => new PipelineState(true, new string[] {});

    public static PipelineState WithError(string error) => new PipelineState(false, new string[] { error });

    public string? Error => AllErrors.FirstOrDefault();
}