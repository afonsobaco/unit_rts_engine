using UnityEngine;
using System.Collections.Generic;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectionArgsXP : ISelectionArgsXP
    {
        private readonly SelectionModifierArguments modifierArgs;
        private readonly SelectionArguments arguments;
        private SelectionResult result;

        public SelectionArgsXP(SelectionArguments arguments, SelectionModifierArguments modifierArgs)
        {
            this.arguments = arguments;
            this.modifierArgs = modifierArgs;
            result.ToBeAdded = Arguments.NewSelection;
        }

        public SelectionArguments Arguments { get => arguments; }
        public SelectionModifierArguments ModifierArgs { get => modifierArgs; }
        public SelectionResult Result { get => result; set => result = value; }
    }

    public struct SelectionModifierArguments
    {
        public SelectionModifierArguments(bool isSameType, bool isAdditive, Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            this.IsAdditive = isAdditive;
            this.IsSameType = isSameType;
            this.InitialScreenPosition = initialScreenPosition;
            this.FinalScreenPosition = finalScreenPosition;
        }

        public bool IsAdditive { get; }
        public bool IsSameType { get; }
        public Vector2 InitialScreenPosition { get; }
        public Vector2 FinalScreenPosition { get; }
    }


    public struct SelectionArguments
    {

        public SelectionArguments(SelectionTypeEnum selectionType, bool isPreSelection, List<ISelectable> oldSelection, List<ISelectable> newSelection, List<ISelectable> mainList)
        {
            SelectionType = selectionType;
            IsPreSelection = isPreSelection;
            OldSelection = oldSelection != null ? oldSelection : new List<ISelectable>();
            NewSelection = newSelection != null ? newSelection : new List<ISelectable>();
            MainList = mainList;

        }

        public SelectionTypeEnum SelectionType { get; }
        public bool IsPreSelection { get; }

        public List<ISelectable> OldSelection { get; }
        public List<ISelectable> NewSelection { get; }
        public List<ISelectable> MainList { get; }
    }

    public struct SelectionResult
    {
        private List<ISelectable> toBeAdded;

        public List<ISelectable> ToBeAdded { get => toBeAdded; set => toBeAdded = value; }
    }

}