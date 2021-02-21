using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using Zenject;

public class MiniatureClass : ISelectable, IGroupable
{
    private string type;

    public int Index { get; set; }
    public bool IsSelected { get; set; }
    public bool IsPreSelected { get; set; }
    public Vector3 Position { get; set; }
    public string Type { get => type; set => type = value; }
    public bool IsHighlighted { get; set; }

    public int CompareTo(object obj)
    {
        return 0;
    }

    public bool IsCompatible(object other)
    {
        if (other == null || GetType() != other.GetType() || !(other is IGroupable))
        {
            return false;
        }
        return this.Type.Equals((other as MiniatureClass).Type);
    }

}




