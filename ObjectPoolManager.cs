using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPool> objectPools;

    public GameObject TryCreate(GameObject newGameObject, Vector3 position, Quaternion rotation)
    {
        //If any object pool contains the newGameObject
        if (objectPools.Any(i=>i.Original == newGameObject))
        {
            ObjectPool targetPool = findPool(newGameObject);
            if (targetPool.objectsInPool.Count>0)
            {
                if (targetPool.objectsInPool[0]!=null)
                {
                    GameObject newGo = targetPool.objectsInPool[0];
                    newGo.SetActive(true);
                    newGo.transform.position = position;
                    newGo.transform.rotation = rotation;
                    targetPool.objectsInPool.Remove(targetPool.objectsInPool[0]);
                    return newGo;
                }
                return null;
            }
            return null;
        }
        else
        {
            Debug.Log("A pool doesn't exist!");
            return null;
        }
    }

    private ObjectPool findPool(GameObject go)
    {
        foreach(ObjectPool op in objectPools)
        {
            if (op.Original == go)
            {
                return op;
            }
        }
        return null;
    }

    public void TryDestroy(GameObject gameObjecToDestroy)
    {

    }


    [System.Serializable]
    public class ObjectPool
    {
        public GameObject Original;
        public List<GameObject> objectsInPool;
    }
}


