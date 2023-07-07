using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    private List<GameObject> pooledObjects = new List<GameObject>();
    private List<GameObject> bloodObjects = new List<GameObject>();

    [SerializeField] private int maxBloodAmount = 1000;
    private void Awake()
    {
        Instance = this;
    }
    public void ResetPool()
    {
        pooledObjects.Clear();
        bloodObjects.Clear();
    }
    public GameObject GetPooledObject(GameObject obj, Vector3 pos, Quaternion rot)
    {
        string objName = obj.name + "(Clone)";
        if (!AllObjectsInUse(objName))
        {
            GameObject returnObj = pooledObjects.First(g => !g.activeSelf && g.name == objName);

            returnObj.transform.SetPositionAndRotation(pos, rot);

            returnObj.SetActive(true);
            return returnObj;
        }
        else
        {
            GameObject newObj = Instantiate(obj);

            newObj.transform.SetPositionAndRotation(pos, rot);

            pooledObjects.Add(newObj);
            return newObj;
        }
    }
    public GameObject AddToBloodPool(GameObject obj, Vector3 pos, Quaternion rot)
    {
        GameObject returnObj;
        if(bloodObjects.Count >= maxBloodAmount)
        {
            returnObj = bloodObjects[0];
            bloodObjects.RemoveAt(0);
            bloodObjects.Add(returnObj);
            returnObj.transform.SetPositionAndRotation(pos, rot);
        }
        else
        {
            returnObj = Instantiate(obj, pos, rot);
            bloodObjects.Add(returnObj);
        }
        return returnObj;
    }
    bool AllObjectsInUse(string name)
    {
        if (pooledObjects.Count > 0)
            return pooledObjects.FindAll(g => g.name == name).All(g => g.activeSelf);
        return true;
    }
}