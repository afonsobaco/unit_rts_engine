using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface IBaseSelectionMod
    {
        bool Active { get; set; }
        bool ActiveOnPreSelection { get; set; }

        ISelectionModifier SelectionModifier { get; set; }
        SelectionTypeEnum Type { get; set; }
        SelectionArgsXP Apply(SelectionArgsXP args);

    }

}
