using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Selection.Tests
{
    public class SelectionManagerTestUtils
    {
        public static List<SelectableObjectTestStruct> GetDefaultTestListOfObjects()
        {
            return new List<SelectableObjectTestStruct>(){
                new SelectableObjectTestStruct(new Vector3(0,0,0), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(1,0,1), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(-1,0,1), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(-1,0,-1), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(2,0,3), SelectableTypeEnum.UNIT),
                new SelectableObjectTestStruct(new Vector3(4,0,3), SelectableTypeEnum.BUILDING),
                new SelectableObjectTestStruct(new Vector3(2,0,-3), SelectableTypeEnum.BUILDING),
                new SelectableObjectTestStruct(new Vector3(1,0,4), SelectableTypeEnum.ENVIRONMENT),
                new SelectableObjectTestStruct(new Vector3(4,0,-2), SelectableTypeEnum.CONSUMABLE)
            };
        }


        public static T CreateATestableObject<T>(int i) where T : MonoBehaviour
        {
            GameObject go = new GameObject();
            var t = go.AddComponent<T>();
            var bc = go.AddComponent<BoxCollider>();
            t.transform.position = GetDefaultTestListOfObjects()[i].pos;
            return t;
        }

        public static List<T> GetDefaultTestMainList<T>() where T : MonoBehaviour
        {
            List<T> mainList = new List<T>();

            for (var i = 0; i < GetDefaultTestListOfObjects().Count; i++)
            {
                var t = CreateATestableObject<T>(i);
                mainList.Add(t);
            }
            return mainList;
        }
    }
}

public struct SelectableObjectTestStruct
{
    internal Vector3 pos;
    internal SelectableTypeEnum typeEnum;

    internal SelectableObjectTestStruct(Vector3 pos, SelectableTypeEnum typeEnum)
    {
        this.pos = pos;
        this.typeEnum = typeEnum;
    }
}

