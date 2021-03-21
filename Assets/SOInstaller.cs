using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
{
    public override void InstallBindings()
    {
        var _subContainer = Container.CreateSubContainer();
        _subContainer.Bind<MyTest>().FromNew().AsSingle().NonLazy();
        //_subContainer.Resolve<MyTest>(); // <-------- need this to start 'MyTest'...
    }
}

public class MyTest
{
    public MyTest(){
         Debug.Log("Created");
    }
}


