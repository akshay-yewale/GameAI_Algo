using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OctNode  {
    public static int maxObjectLimit = 1; // max objects that can be stored before birth of 8 children

    static OctNode _OctTreeRootNode;
    public static OctNode octTreeRootNode
    {
        get
        {
            if (_OctTreeRootNode == null)
            {
                _OctTreeRootNode = new OctNode(null, Vector3.zero, 15.0f, new List<OctreeItem>());
            }
            return _OctTreeRootNode;
        }
    }

    GameObject octantGameObject;
    LineRenderer octantLinerenderer;


    public float halfDimensionLenght;
    private Vector3 position;
    public OctNode parent;

    public List<OctreeItem> containedItems = new List<OctreeItem>();
    private OctNode[] children = new OctNode[8];

    public OctNode[] ChildrenNodes
    {
        get
        {
            return children;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    static bool Initialize()
    {
        Debug.Log("OctRoot is initialized");
        return octTreeRootNode == null;
    }

    //Default Contrustor
    public OctNode(OctNode parent, Vector3 this_Childposition, float halfDimensionLenght,  List<OctreeItem> potentialItems)
    {
        this.parent = parent;
        this.position = this_Childposition;
        this.halfDimensionLenght = halfDimensionLenght;

        octantGameObject = new GameObject();
        octantGameObject.hideFlags = HideFlags.HideInHierarchy;
        octantLinerenderer = octantGameObject.AddComponent<LineRenderer>();

        FillCube_VisualizeCordinate();
        
        foreach(OctreeItem octreeItem in potentialItems)
        {
            ProcessItem(octreeItem);
        }
    }

    public bool ProcessItem(OctreeItem item)
    {
        if(ContainsItemPostion(item.transform.position))
        {
            if(ReferenceEquals(ChildrenNodes[0],null))
            {
                PushItem(item);
                return true;
            }
            else
            {
                foreach(OctNode child in ChildrenNodes)
                {
                    if (child.ProcessItem(item))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void PushItem(OctreeItem item)
    {
        if(!containedItems.Contains(item))
        {
            containedItems.Add(item);
            item.octreeNodes.Add(this);

        }
        if(containedItems.Count > maxObjectLimit)
        {
            Split();

            containedItems.Clear();
        }
    }

    void Split()
    {
        foreach(OctreeItem item in containedItems)
        {
            item.octreeNodes.Remove(this);
        }
        Vector3 positionVector = new Vector3(halfDimensionLenght / 2, halfDimensionLenght / 2, halfDimensionLenght / 2);
        for(int index = 0; index < 4; index++)
        {
            children[index] = new OctNode(this, position + positionVector, halfDimensionLenght / 2, containedItems);
            positionVector = Quaternion.Euler(0.0f, -90.0f, 0.0f) * positionVector;
        }
        positionVector = new Vector3(halfDimensionLenght / 2, -halfDimensionLenght / 2, halfDimensionLenght / 2);
        for (int index = 4; index < 8; index++)
        {
            children[index] = new OctNode(this, position + positionVector, halfDimensionLenght / 2, containedItems);
            positionVector = Quaternion.Euler(0.0f, -90.0f, 0.0f) * positionVector;
        }
    }

    void FillCube_VisualizeCordinate()
    {
        Vector3[] cubeCoords = new Vector3[8];
        Vector3 corner = new Vector3(halfDimensionLenght, halfDimensionLenght, halfDimensionLenght);
        for(int index = 0; index <4; index++)
        {
            cubeCoords[index] = position + corner;
            corner = Quaternion.Euler(0, 90f, 0f) * corner;
        }
        
        corner = new Vector3(halfDimensionLenght, -halfDimensionLenght, halfDimensionLenght);
        for (int index = 4; index < 8; index++)
        {
            cubeCoords[index] = position + corner;
            corner = Quaternion.Euler(0, 90f, 0f) * corner;
        }

        octantLinerenderer.useWorldSpace = true;
        octantLinerenderer.positionCount = 16;
        octantLinerenderer.startWidth = 0.05f;
        octantLinerenderer.endWidth = 0.05f;

        octantLinerenderer.SetPosition(0, cubeCoords[0]);
        octantLinerenderer.SetPosition(1, cubeCoords[1]);
        octantLinerenderer.SetPosition(2, cubeCoords[2]);
        octantLinerenderer.SetPosition(3, cubeCoords[3]);
        octantLinerenderer.SetPosition(4, cubeCoords[0]);
        octantLinerenderer.SetPosition(5, cubeCoords[4]);
        octantLinerenderer.SetPosition(6, cubeCoords[5]);
        octantLinerenderer.SetPosition(7, cubeCoords[1]);

        octantLinerenderer.SetPosition(8, cubeCoords[5]);
        octantLinerenderer.SetPosition(9, cubeCoords[6]);
        octantLinerenderer.SetPosition(10, cubeCoords[2]);
        octantLinerenderer.SetPosition(11, cubeCoords[6]);
        octantLinerenderer.SetPosition(12, cubeCoords[7]);
        octantLinerenderer.SetPosition(13, cubeCoords[3]);
        octantLinerenderer.SetPosition(14, cubeCoords[7]);
        octantLinerenderer.SetPosition(15, cubeCoords[4]);

    }

    bool ContainsItemPostion(Vector3 itemPosition)
    {

        if (itemPosition.x > position.x + halfDimensionLenght || itemPosition.x < position.x - halfDimensionLenght)
            return false;
        if (itemPosition.y > position.y + halfDimensionLenght || itemPosition.y < position.y - halfDimensionLenght)
            return false;
        if (itemPosition.z > position.z + halfDimensionLenght || itemPosition.z < position.z - halfDimensionLenght)
            return false;

        return true;
    }
}
