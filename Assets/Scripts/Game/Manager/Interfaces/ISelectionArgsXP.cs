using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface ISelectionArgsXP
    {
        SelectionArguments Arguments { get; }
        SelectionModifierArguments ModifierArgs { get; }
    }

}