using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public List<LevelGameObjectBase> LevelGameObjectList = new List<LevelGameObjectBase>();


    private static ResourceManager instance = null;
    public static ResourceManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public LevelGameObjectBase GetObjBase(string obj_Id)
    {
        LevelGameObjectBase retVal = null;
        for (int i = 0; i < LevelGameObjectList.Count; i++)
        {
            if (obj_Id.Equals(LevelGameObjectList[i].objectID))
            {
                retVal = LevelGameObjectList[i];
                break;
            }
        }
        return retVal;
    }

    public LevelGameObjectBase GetObjectForMousePointer(string object_id)
    {
        LevelGameObjectBase retVal = null;
        for (int i = 0; i < LevelGameObjectList.Count; i++)
        {
            if (object_id.Equals(LevelGameObjectList[i].objectID))
            {
                retVal = LevelGameObjectList[i];
                break;
            }
        }
        return retVal;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


[System.Serializable]
public class LevelGameObjectBase
{
    public string objectID;
    public GameObject objPrefab;
    public GameObject objectPrefabForMousePointer;
}