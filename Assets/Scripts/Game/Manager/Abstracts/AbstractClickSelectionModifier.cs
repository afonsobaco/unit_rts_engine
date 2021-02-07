namespace RTSEngine.Manager
{
    public abstract class AbstractClickSelectionModifier : ISelectionModifier
    {
        public SelectionTypeEnum Type { get { return SelectionTypeEnum.CLICK; } }
        public abstract SelectionArguments Apply(SelectionArguments args);
    }
}
