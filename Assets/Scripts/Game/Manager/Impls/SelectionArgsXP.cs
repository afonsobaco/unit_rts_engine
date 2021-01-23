using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public interface ISelectionArgsXP<T, E>
    {
        List<T> OldSelection { get; set; }
        List<T> NewSelection { get; set; }
        List<T> ToBeAdded { get; set; }
        List<T> ToBeRemoved { get; set; }
        E SelectionType { get; set; }
        List<ISelectionMod<T, E>> SelectionModifiers { get; set; }
    }

    public class SelectionArgsXP<T, E> : ISelectionArgsXP<T, E>
    {
        private List<T> oldSelection = new List<T>();
        private List<T> newSelection = new List<T>();
        private List<T> toBeAdded = new List<T>();
        private List<T> toBeRemoved = new List<T>();
        private E selectionType;

        private List<ISelectionMod<T, E>> selectionModifiers = new List<ISelectionMod<T, E>>();

        public List<T> OldSelection { get => oldSelection; set => oldSelection = value; }
        public List<T> NewSelection { get => newSelection; set => newSelection = value; }
        public List<T> ToBeAdded { get => toBeAdded; set => toBeAdded = value; }
        public List<T> ToBeRemoved { get => toBeRemoved; set => toBeRemoved = value; }
        public E SelectionType { get => selectionType; set => selectionType = value; }
        public List<ISelectionMod<T, E>> SelectionModifiers { get => selectionModifiers; set => selectionModifiers = value; }
    }

}