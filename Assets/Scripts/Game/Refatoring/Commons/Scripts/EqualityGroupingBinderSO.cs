using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Commons
{
    [CreateAssetMenu(fileName = "EqualityGroupingBinderSO", menuName = "Installers/Equality Grouping Binder")]
    public class EqualityGroupingBinderSO : ScriptableObjectInstaller<EqualityGroupingBinderSO>
    {
        [SerializeField] private EqualityComparerComponent equalityComparer;
        [SerializeField] private GroupingComparerComponent groupingComparer;

        public EqualityComparerComponent EqualityComparer { get => equalityComparer; set => equalityComparer = value; }
        public GroupingComparerComponent GroupingComparer { get => groupingComparer; set => groupingComparer = value; }

        public override void InstallBindings()
        {
            Container.Bind<EqualityComparerComponent>().FromComponentInNewPrefab(equalityComparer).AsSingle();
            Container.Bind<GroupingComparerComponent>().FromComponentInNewPrefab(groupingComparer).AsSingle();
        }
    }
}