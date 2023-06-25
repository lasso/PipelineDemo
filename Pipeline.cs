public class Pipeline<T> : IPipe<T>
{
    private static bool DefaultBreaker((T, PipelineState) result) => !result.Item2.Success;
    private Predicate<(T, PipelineState)> _breaker = DefaultBreaker;
    private IReadOnlyList<IPipe<T>> _pipes;

    public Pipeline(IEnumerable<IPipe<T>> pipes) => _pipes = pipes.ToArray();

    public Pipeline(IEnumerable<IPipe<T>> pipes, Predicate<(T, PipelineState)> breaker)
    {
        _breaker = breaker;
        _pipes = pipes.ToArray();
    }

    public (T, PipelineState) process(T input)
    {
        (T Input, PipelineState State) current = (input, PipelineState.Ok);

        foreach (var pipe in _pipes)
        {
            current = pipe.process(current.Input);

            if (_breaker(current))
            {
                Console.Out.WriteLine($"Stopped after running {pipe.GetType()}.");
                break;
            }
            else {
                Console.Out.WriteLine($"Successfully completed {pipe.GetType()}.");
            }
        }

        return current;
    }
}