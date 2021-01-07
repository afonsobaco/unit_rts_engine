using System;
using UnityEngine;
using System.Collections.Generic;

public interface SelectionModInterface
{
    List<SelectableObject> Apply(List<SelectableObject> oldList, List<SelectableObject> newList, SelectableObject clicked);
}