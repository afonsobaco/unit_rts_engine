using RTSEngine.Manager.Impls;

namespace RTSEngine.Manager.Interfaces
{
    public interface IAbstractSelectionMod<T, ST>
    {
        bool Active { get; set; }
        ST Type { get; set; }

        SelectionArgsXP<T, ST> Apply(SelectionArgsXP<T, ST> args);
    }
}
