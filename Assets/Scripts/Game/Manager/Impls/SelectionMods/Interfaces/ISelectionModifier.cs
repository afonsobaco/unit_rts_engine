namespace RTSEngine.Manager
{

    public interface ISelectionModifier
    {
        SelectionTypeEnum Type { get; }
        SelectionArgsXP Apply(SelectionArgsXP args);
    }

}