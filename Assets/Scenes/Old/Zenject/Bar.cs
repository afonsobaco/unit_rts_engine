using Zenject;
using UnityEngine;

public class Bar : MonoBehaviour
{

    private Foo foo;
    [Inject]
    private void Construct(Foo foo)
    {
        Debug.Log("Bar construct");
        this.foo = foo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Foo: " + foo);
        }
    }

}