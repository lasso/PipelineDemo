public class AuthenticationPipe : IPipe<Message>
{
    private const string EXPECTED_TOKEN = "YTE5Nzk0ZDYtMGJiYy00MjIzLTlhYzUtYjJmMzViNGNkOWZh";

    public (Message, PipelineState) process(Message input)
    {
        if (input.Token == null || !input.Token.Equals(EXPECTED_TOKEN, StringComparison.InvariantCultureIgnoreCase))
        {
             return (input, PipelineState.WithError("Unauthenticated"));
        }
        return (input, PipelineState.Ok);
    }
}