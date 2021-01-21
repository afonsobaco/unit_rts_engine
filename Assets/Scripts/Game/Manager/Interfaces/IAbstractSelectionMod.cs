using RTSEngine.Manager.Impls;

namespace RTSEngine.Manager.Interfaces
{
    public interface IAbstractSelectionMod<T, S, O>
    {
        bool Active { get; set; }
        S Type { get; set; }

        SelectionArgsXP<T, S, O> Apply(SelectionArgsXP<T, S, O> args);
    }
}
