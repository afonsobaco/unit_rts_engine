namespace RTSEngine.Manager
{
    public abstract class AbstractSelectionModifier : ISelectionModifier
    {
        public SelectionTypeEnum Type { get { return SelectionTypeEnum.ANY; } }
        public abstract SelectionArgsXP Apply(SelectionArgsXP args);
    }
}
