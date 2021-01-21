using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Impls
{
    public class SelectionArgsXP<T, E>
    {
        private List<T> oldSelection = new List<T>();
        private List<T> newSelection = new List<T>();
        private List<T> toBeAdded = new List<T>();
        private List<T> toBeRemoved = new List<T>();
        private E selectionType;
        private ISelectionSettings<T, E> settings;

        public List<T> OldSelection { get => oldSelection; set => oldSelection = value; }
        public List<T> NewSelection { get => newSelection; set => newSelection = value; }
        public List<T> ToBeAdded { get => toBeAdded; set => toBeAdded = value; }
        public List<T> ToBeRemoved { get => toBeRemoved; set => toBeRemoved = value; }
        public E SelectionType { get => selectionType; set => selectionType = value; }
        public ISelectionSettings<T, E> Settings { get => settings; set => settings = value; }
    }

}