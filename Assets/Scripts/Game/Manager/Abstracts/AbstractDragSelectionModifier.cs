namespace RTSEngine.Manager
{
    public abstract class AbstractDragSelectionModifier : ISelectionModifier, IPreSelectionModifier
    {
        public SelectionTypeEnum Type { get { return SelectionTypeEnum.DRAG; } }
        public virtual bool ActiveOnPreSelection { get { return true; } }

        public abstract SelectionArgsXP Apply(SelectionArgsXP args);
    }
}
