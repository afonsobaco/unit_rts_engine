namespace RTSEngine.Manager
{

    public interface ISelectionModifier
    {
        SelectionTypeEnum Type { get; }
        SelectionArguments Apply(SelectionArguments args);
    }

}