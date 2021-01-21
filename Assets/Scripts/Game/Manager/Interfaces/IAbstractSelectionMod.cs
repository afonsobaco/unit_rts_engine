using RTSEngine.Manager.Impls;

namespace RTSEngine.Manager.Interfaces
{
    public interface IAbstractSelectionMod<T, E>
    {
        bool Active { get; set; }
        E Type { get; set; }

        SelectionArgsXP<T, E> Apply(SelectionArgsXP<T, E> args);
    }
}
