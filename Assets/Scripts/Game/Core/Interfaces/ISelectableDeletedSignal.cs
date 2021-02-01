namespace RTSEngine.Core
{
    public interface ISelectableDeletedSignal<T>
    {
        T Selectable { get; set; }
    }
}
