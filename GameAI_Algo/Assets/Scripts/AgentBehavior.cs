using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehavior : MonoBehaviour {

    public Dictionary<Node, Node> nodeParent = new Dictionary<Node, Node>();
    public Node result;

    public Material mat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ShowBFSPath()
    {
       
        Node start = GridBase.GetInstance().NodeFromWorldPosition(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);
        Node goal = GridBase.GetInstance().NodeFromWorldPosition(GameObject.FindGameObjectWithTag("Goal").gameObject.transform.position);

        result = GetShortestPathBFS(start, goal);

        if(result == start)
        {
            Debug.Log("BFS IS SUX");
        }
        else
        {
            Debug.Log("BFS IS NOT SUX");
        }
        Node cur = result;

        while(cur != start)
        {
            cur.tileMeshRenderer.material = mat;
            cur = nodeParent[cur];
        }
    }


    //BFS Implementation
    Node GetShortestPathBFS(Node startPosition, Node GoalNode)
    {
        Queue<Node> queue = new Queue<Node>();
        Queue<Node> exploredNodes = new Queue<Node>();

        queue.Enqueue(startPosition);

        while (queue.Count != 0)
        {
            Node node = queue.Dequeue();
            if(node == GoalNode)
            {
                return node;
            }

            List<Node> adjacentWalkableNodes = GridBase.GetInstance().GetWalkableAdjacentNodes(node);

            foreach(Node t_node in adjacentWalkableNodes)
            {
                if (!exploredNodes.Contains(node))
                {
                    exploredNodes.Enqueue(node);
                    nodeParent.Add(t_node, node);
                    queue.Enqueue(t_node);
                }
            }
        }

        return startPosition;
    }
}
