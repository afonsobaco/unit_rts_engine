using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionModScriptaleObject : ScriptableObject, SelectionModInterface
{
    public abstract List<SelectableObject> Apply(List<SelectableObject> oldList, List<SelectableObject> newList, SelectableObject clicked);
}
