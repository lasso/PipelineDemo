using System.Text.Json;

var message = JsonSerializer.Deserialize<Message>(File.OpenRead("message.json"), new JsonSerializerOptions(JsonSerializerDefaults.Web));

if (message == null)
{
    Console.Error.WriteLine("Failed to parse JSON file.");
    Environment.Exit(1);
}

var pipes = new IPipe<Message>[] {
    new AuthenticationPipe(),
    new AuthorizationPipe(),
    new SetBatchNoPipe()
};

var pipeline = new Pipeline<Message>(pipes);

(Message Message, PipelineState State) result = pipeline.process(message);

Console.Out.WriteLine(result);
