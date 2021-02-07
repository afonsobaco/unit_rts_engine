using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class TestInstaller : MonoInstaller
    {
        [SerializeField] private string text = "";
        private IClassManager _manager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ClassManager>().AsSingle().NonLazy();
            // Container.Bind<IClassManager>().To<ClassManager>().AsCached().OnInstantiated<IClassManager>(Initiate).NonLazy();
            Container.Bind<ClassA>().FromNewComponentOnNewGameObject().WithGameObjectName("Class A").AsSingle().NonLazy();

            Container.DeclareSignal<SignalA>();
            Container.DeclareSignal<SignalB>();
            Container.BindSignal<SignalA>().ToMethod<ClassManager>(x => x.SignalA).FromResolve();
            Container.BindSignal<SignalB>().ToMethod<ClassManager>(x => x.SignalB).FromResolve();
        }

        private void Initiate(InjectContext ctx, IClassManager manager)
        {
            Debug.Log(manager);
            this._manager = manager;
        }

        private void OnValidate()
        {
            if (this._manager != null)
                this._manager.Text = text;
        }
    }

    public class ClassA : MonoBehaviour
    {
        private ClassManager _manager;

        [Inject]
        public void Constructor(ClassManager manager)
        {
            Debug.Log("Constructor Class A");
            this._manager = manager;
        }

    }

    public interface IClassManager
    {
        string Text { get; set; }

        void Construct(SignalBus signalBus);
        void SignalA();
        void SignalB();
    }

    public class ClassManager : IClassManager
    {
        private SignalBus _signalBus;
        public string Text { get; set; }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            Debug.Log("Constructor Class Manager");
            this._signalBus = signalBus;
        }

        public void SignalA()
        {
            Debug.Log("Signal A : " + Text);
        }
        public void SignalB()
        {
            Debug.Log("Signal B: " + Text);
        }
    }


    public class SignalA
    {
        public UnitTestScript Selectable { get; internal set; }
    }
    public class SignalB
    {
        public UnitTestScript Selectable { get; internal set; }
    }
}