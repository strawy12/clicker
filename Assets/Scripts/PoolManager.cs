using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPoolingType { ClickEffect, CoinText, CottenCandy }

[System.Serializable]
public class ObjectPoolData
{
    public List<PoolObject> prefabs = new List<PoolObject>();

    public List<int> prefabCreateCounts = new List<int>();
}

public class PoolManager : MonoBehaviour
{
    private Dictionary<EPoolingType, Stack<PoolObject>> dictPoolList = new Dictionary<EPoolingType, Stack<PoolObject>>();

    [SerializeField] ObjectPoolData objectPoolData = null;

    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        int dictCount = objectPoolData.prefabs.Count;
        int objectCount = 0;
        EPoolingType type;
        PoolObject poolObject = null;

        for (int i = 0; i < objectPoolData.prefabs.Count; i++)
        {
            type = (EPoolingType)i;
            objectCount = objectPoolData.prefabCreateCounts[i];

            dictPoolList.Add(type, new Stack<PoolObject>());

            for (int j = 0; j < objectCount; j++)
            {
                poolObject = GerenationPoolObject(type);
                dictPoolList[type].Push(poolObject);
            }
        }
    }

    private PoolObject GerenationPoolObject(EPoolingType type)
    {
        int index = (int)type;
        PoolObject poolObject = Instantiate(objectPoolData.prefabs[index], transform);
        poolObject.gameObject.SetActive(false);

        return poolObject;
    }

    public PoolObject GetPoolObject(EPoolingType type)
    {
        if (dictPoolList[type].Count != 0)
        {
            return dictPoolList[type].Pop();
        }

        else
        {
            return GerenationPoolObject(type);
        }
    }
    public void PushPoolObject(PoolObject poolObject)
    {
        EPoolingType type = poolObject.PoolType;
        dictPoolList[type].Push(poolObject);
    }
}
