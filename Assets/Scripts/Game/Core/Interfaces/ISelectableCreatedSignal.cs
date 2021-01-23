namespace RTSEngine.Core
{
    public interface ISelectableCreatedSignal<T>
    {
        T Selectable { get; set; }
    }
}
