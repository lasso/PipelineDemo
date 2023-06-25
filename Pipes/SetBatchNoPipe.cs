
public class SetBatchNoPipe : IPipe<Message>
{
    private static int YX_LAST_BATCH = 123;
    private static int YW_LAST_BATCH = 345;

    public (Message, PipelineState) process(Message input)
    {
        var customer = input.CustomerCode?.Split("-").FirstOrDefault();

        switch (customer)
        {
            case "YX":
                input = input with { BatchNo = ++YX_LAST_BATCH };
                return (input, PipelineState.Ok);
            case "YW":
                input = input with { BatchNo = ++YW_LAST_BATCH };
                return (input, PipelineState.Ok);
            default:
                return (input, PipelineState.WithError("Unknown customer"));
        }
    }
}