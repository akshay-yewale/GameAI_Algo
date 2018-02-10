using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBase : MonoBehaviour {

    public GameObject nodePrefab;
    public float startX;
    public float startZ;
    public int sizeX;
    public int sizeZ;
    public float offset = 2;
    public float subtractionToOffsetValue = 1;

    public Node[,] grid;

    private static GridBase instance = null;

    public static GridBase GetInstance()
    {
        return instance;
    }
    // Use this for initialization

    void Awake()
    {
        instance = this;
        CreateGrid();
        CreateMouseCollision();
    }


    void Start () {
		
	}

    private void CreateGrid()
    {
        grid = new Node[sizeX, sizeZ];

        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {

                float posX = x * offset + startX;
                float posZ = z * offset + startZ;

                GameObject go = Instantiate(nodePrefab, new Vector3(posX, 0, posZ), Quaternion.identity) as GameObject;
                go.transform.SetParent(this.transform);
                    
                Node node = new Node();
                node.visualizer = go;
                node.tileMeshRenderer = node.visualizer.GetComponentInChildren<MeshRenderer>();
                node.isWalkable = true;
                node.nodePositionX = x;
                node.nodePositionZ = z;
                grid[x, z] = node;
              
            }
        }
    }

    // Update is called once per frame

    void CreateMouseCollision()
    {
        GameObject go = new GameObject();
        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().size = new Vector3(sizeX * offset, 0.01f, sizeZ * offset);
        go.transform.position = new Vector3((sizeX * offset) / 2 + startX - subtractionToOffsetValue, 0, (sizeZ * offset) / 2 + startZ - subtractionToOffsetValue);

    }

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {

        float worldX = worldPosition.x - startX;
        float worldZ = worldPosition.z - startZ;

        worldX /= offset;
        worldZ /= offset;

        int x = Mathf.RoundToInt(worldX);
        int z = Mathf.RoundToInt(worldZ);


        if (x >= sizeX)
            x = sizeX - 1;
        else if (x < 0)
            x = 0;

        if (z >= sizeZ)
            z = sizeZ - 1;
        else if (z < 0)
            z = 0;

        return grid[x, z];
    }

    public List<Node> GetWalkableAdjacentNodes(Node node)
    {
        List<Node> result = new List<Node>();

        List<Node> possibleNodes = new List<Node>();

        if(node.nodePositionX !=0)
        {
        possibleNodes.Add(grid[node.nodePositionX - 1, node.nodePositionZ]);

        }

        if (node.nodePositionZ != 0)
            possibleNodes.Add(grid[node.nodePositionX,node.nodePositionZ-1]);

        if (node.nodePositionX < sizeX-1)
            possibleNodes.Add(grid[node.nodePositionX+1, node.nodePositionZ]);

        if (node.nodePositionZ < sizeZ - 1)
            possibleNodes.Add(grid[node.nodePositionX , node.nodePositionZ +1]);


        foreach(Node t_node in possibleNodes)
        {
            if (t_node.isWalkable == true)
                result.Add(t_node);
        }
        return result;
    }

    public List<Node> GetAllWalkableNodes()
    {
        List<Node> result = new List<Node>();
        foreach(Node node in grid)
        {
            if (node.isWalkable)
                result.Add(node);
        }
        return result;
    }

}
