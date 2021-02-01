namespace RTSEngine.Manager
{

    public interface ISelectionModifier
    {
        SelectionTypeEnum Type { get; }
        bool ActiveOnPreSelection { get; }

        SelectionArgsXP Apply(SelectionArgsXP args);
    }

}