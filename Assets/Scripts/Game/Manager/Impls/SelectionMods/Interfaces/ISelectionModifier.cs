namespace RTSEngine.Manager
{

    public interface ISelectionModifier
    {
        SelectionArgsXP Apply(SelectionArgsXP args, params object[] other);
    }

}