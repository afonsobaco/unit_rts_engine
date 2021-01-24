using RTSEngine.Core;
namespace RTSEngine.Manager
{
    public class SameTypeUtil
    {
        // public static List<ISelectable> GetFromSameTypeInSelection(SelectionArgs args, Vector2 initialGameScreenPos, Vector2 finalGameScreenPos)
        // {
        //     List<ISelectable> list = SelectionUtil.FindAllOnScreen(args, initialGameScreenPos, finalGameScreenPos);
        //     list.RemoveAll(a => !isSameType(args.Clicked, a));
        //     return list;
        // }

        public static bool isSameType(ISelectable first, ISelectable second)
        {
            return first.IsCompatible(second);
        }
    }
}
