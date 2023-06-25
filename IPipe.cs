public interface IPipe<T>
{
    public (T, PipelineState) process(T input);
}