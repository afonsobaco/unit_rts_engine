namespace RTSEngine.Manager
{
    public class SameTypeUtil
    {
        // public static List<SelectableObject> GetFromSameTypeInSelection(SelectionArgs args, Vector2 initialGameScreenPos, Vector2 finalGameScreenPos)
        // {
        //     List<SelectableObject> list = SelectionUtil.FindAllOnScreen(args, initialGameScreenPos, finalGameScreenPos);
        //     list.RemoveAll(a => !isSameType(args.Clicked, a));
        //     return list;
        // }

        public static bool isSameType(SelectableObject first, SelectableObject second)
        {
            return second.Type == first.Type && second.TypeStr.Equals(first.TypeStr);
        }
    }
}
