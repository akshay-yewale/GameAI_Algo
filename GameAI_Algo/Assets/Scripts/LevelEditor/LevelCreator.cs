using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour {

    public new Camera camera ;
	
	GridBase gridBase;
	InterfaceManager uiManager;

	bool hasObj;
	GameObject objToPlace;
	GameObject cloneObj;
	//Level_Object objProperties;
	Vector3 mousePosition;
	Vector3 worldPosition;
	bool deleteObj;

	public bool placeStackObj;
	GameObject stackableObjToPlace;
	GameObject stackableObjCloneObj;
    GameObject stackableCloneObjectForMousePointer;

   
	bool deleteStackableObj;

    private static LevelCreator instance = null;
    public static LevelCreator GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
		gridBase = GridBase.GetInstance ();
		uiManager = InterfaceManager.GetInstance ();
		

	}
	
	// Update is called once per frame
	void Update () {
		PlaceStackedObjects ();
       
	}

	void UpdateMousePosition(){

        //Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
			mousePosition = hit.point;
		}

	}

	void CloseAll (){
	
		hasObj = false;
		deleteObj = false;
		placeStackObj = false;
		deleteStackableObj = false;

	}

	public void PassGameObjectToPlace(string obj_id)
	{
		
		if (stackableObjCloneObj != null) {
			Destroy (stackableObjCloneObj);
		}

		CloseAll ();
		placeStackObj = true;
		stackableObjCloneObj = null;
		stackableObjToPlace =ResourceManager.GetInstance().GetObjBase (obj_id).objPrefab;
        stackableCloneObjectForMousePointer = ResourceManager.GetInstance().GetObjectForMousePointer(obj_id).objectPrefabForMousePointer;

    }

    public void DeleteStackedObject()
    {
        CloseAll();
        deleteStackableObj = true;
    }



	void PlaceStackedObjects(){

		if (placeStackObj) {
		
			UpdateMousePosition ();


			Node curNode = gridBase.NodeFromWorldPosition (mousePosition);

			worldPosition = curNode.visualizer.transform.transform.position;            
			if (stackableObjCloneObj == null) {
			
                
				stackableObjCloneObj = Instantiate (stackableCloneObjectForMousePointer, worldPosition, Quaternion.identity) as GameObject;
                //stackableObjCloneObj.tag = "";
                //stackableObjProperties = stackableObjCloneObj.GetComponent <Level_Object> ();


			} else {
			
				stackableObjCloneObj.transform.position = worldPosition;
				if (Input.GetMouseButtonUp (0) && !uiManager.isMouseOverUIElement) {

                    
                    {
                        GameObject actualObjToPlaced = Instantiate(stackableObjToPlace, worldPosition, stackableObjCloneObj.transform.rotation) as GameObject;

                        curNode.placedObject = actualObjToPlaced;
                        if(actualObjToPlaced.tag == "Player" || actualObjToPlaced.tag == "Goal")
                        {
                            curNode.isWalkable = true;
                            Destroy(stackableObjCloneObj.gameObject);
                            placeStackObj = false;
                        }
                        else
                        {
                            curNode.isWalkable = false;
                        }
                       
                        //lvlManager.inSceneStackedGameObjects.Add(actualObjToPlaced);
                       
                    }

                        
				}

				if (Input.GetMouseButtonUp (1)) {
				
					//stackableObjProperties.ChangeRotation ();
				}

			}
		} 

		else {
		
			if (stackableObjCloneObj != null) {
			
				Destroy (stackableObjCloneObj);
			}
		}

	}






}
