using System;
using ModestTree;

namespace Zenject
{
    [NoReflectionBaking]
    public class InstantiateCallbackConditionCopyNonLazyBinder : ConditionCopyNonLazyBinder
    {
        public InstantiateCallbackConditionCopyNonLazyBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public ConditionCopyNonLazyBinder OnInstantiated(
            Action<InjectContext, object> callback)
        {
            BindInfo.InstantiatedCallback = callback;
            return this;
        }

        public ConditionCopyNonLazyBinder OnInstantiated<T>(
            Action<InjectContext, T> callback)
        {
            // Can't do this here because of factory bindings
            //Assert.That(BindInfo.ContractTypes.All(x => x.DerivesFromOrEqual<T>()));

            BindInfo.InstantiatedCallback = (ctx, obj) =>
            {
                if (ctx.Container.IsValidating)
                {
                    Log.Info(obj.ToString());
                    Log.Info(obj.GetType().Name);
                    Type markedType = ((ValidationMarker)obj).MarkedType;
                    Assert.That(markedType.Equals(typeof(T)),
                        "Invalid generic argument to OnInstantiated! {0} must be type {1}", markedType, typeof(T));
                    callback(ctx, default(T));
                }
                else
                {
                    Assert.That(obj == null || obj is T,
                        "Invalid generic argument to OnInstantiated! {0} must be type {1}", obj.GetType(), typeof(T));
                    callback(ctx, (T)obj);
                }

            };
            return this;
        }
    }
}
