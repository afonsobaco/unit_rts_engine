using RTSEngine.Core;
using RTSEngine.Utils;

namespace RTSEngine.Manager
{
    public interface ISelectableObjectBehaviour : ISelectable
    {
        ObjectTypeEnum Type { get; set; }
        SelectionMark SelectionMark { get; set; }
        SelectionMark PreSelectionMark { get; set; }

    }



}
