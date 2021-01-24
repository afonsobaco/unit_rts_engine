namespace RTSEngine.Manager
{
    public interface ISelectionMod<T, ST>
    {
        bool Active { get; set; }
        ST Type { get; set; }
        bool ActiveOnPreSelection { get; set; }

        SelectionArgsXP Apply(SelectionArgsXP args);
    }
}
