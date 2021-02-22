using Zenject;
using UnityEngine;

public class Foo : MonoBehaviour
{
    [Inject]
    private void Construct()
    {
        Debug.Log("Here");
    }

    public class Factory : PlaceholderFactory<Foo>
    {
    }
}