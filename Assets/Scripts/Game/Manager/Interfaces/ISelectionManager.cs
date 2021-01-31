using System;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface ISelectionManager<T, M, ST> : IDisposable where T : ISelectable
    {
        void SetMainList(HashSet<T> list);
        void SetSelctionModifiers(HashSet<M> list);
        void SetScreenBoundries(Vector2 minScreenPoint, Vector2 maxScreenPoint);
        void SetKeysPressed(bool additiveKeyPressed, bool sameTypeKeyPressed);
        void SetGroupNumperPressed(int keyPressed);
        void SetDoubleClick(bool doubleClick);

        bool IsSelecting();
        bool IsAdditive();
        bool IsSameType();

        int GetGroupSetNumberPressed();
        Vector2 GetInitialScreenPosition();
        Vector2 GetFinalScreenPosition();
        HashSet<T> GetCurrentSelection();
        HashSet<T> GetGroupSet(int groupSetId);
        Dictionary<int, HashSet<ISelectableObjectBehaviour>> GetAllGroupSets();
    }
}
