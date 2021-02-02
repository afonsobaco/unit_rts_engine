namespace RTSEngine.Core
{
    public interface ISelectableSignal<T>
    {
        T Selectable { get; set; }
    }
}
