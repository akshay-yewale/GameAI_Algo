using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehavior : MonoBehaviour {

    IDictionary<Node, Node> nodeParent = new Dictionary<Node, Node>();
    public Node result;

    public Material BFSmaterial;
    public Material DFSmaterial;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ShowBFSPath()
    {
        Node start = GridBase.GetInstance().NodeFromWorldPosition(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);// GridBase.GetInstance().grid[0, 0]; //
        Node goal = GridBase.GetInstance().NodeFromWorldPosition(GameObject.FindGameObjectWithTag("Goal").gameObject.transform.position);//GridBase.GetInstance().grid[5, 5];//

        result = GetShortestPathBFS(start, goal);
        Node cur = result;

        while(cur != start)
        {
            cur.tileMeshRenderer.material = BFSmaterial;
            cur = nodeParent[cur];
        }
    }

    public void ShowDFSPath()
    {
        Node start = GridBase.GetInstance().NodeFromWorldPosition(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);// GridBase.GetInstance().grid[0, 0]; //
        Node goal = GridBase.GetInstance().NodeFromWorldPosition(GameObject.FindGameObjectWithTag("Goal").gameObject.transform.position);//GridBase.GetInstance().grid[5, 5];//

        result = GetShortestPathDFS(start, goal);
        Node cur = result;

        while (cur != start)
        {
            cur.tileMeshRenderer.material = DFSmaterial;
            cur = nodeParent[cur];
        }
    }


    //BFS Implementation
    Node GetShortestPathBFS(Node startPosition, Node GoalNode)
    {
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> exploredNodes = new HashSet<Node>();

        queue.Enqueue(startPosition);

        while (queue.Count != 0)
        {
            Node currentNode = queue.Dequeue();
            if(currentNode == GoalNode)
            {
                return currentNode;
            }

            List<Node> adjacentWalkableNodes = GridBase.GetInstance().GetWalkableAdjacentNodes(currentNode);

            foreach(Node node in adjacentWalkableNodes)
            {
                if (!exploredNodes.Contains(node))
                {
                    exploredNodes.Add(currentNode);
                    if(nodeParent.ContainsKey(node))
                    {
                        nodeParent[node] = currentNode;
                    }
                    else
                    {
                        nodeParent.Add(node, currentNode);
                    }
                    
                    queue.Enqueue(node);
                }
            }
        }

        return startPosition;
    }

    Node GetShortestPathDFS(Node startPosition, Node GoalNode)
    {
        Stack<Node> stack = new Stack<Node>();
        HashSet<Node> exploredNodes = new HashSet<Node>();
        stack.Push(startPosition);

        while(stack.Count!=0)
        {
            Node currentNode = stack.Pop();
            if (currentNode == GoalNode)
                return currentNode;

            List<Node> adjacentWalkableNodes = GridBase.GetInstance().GetWalkableAdjacentNodes(currentNode);

            foreach(Node node in adjacentWalkableNodes)
            {
                if(!exploredNodes.Contains(node))
                {
                    exploredNodes.Add(node);
                    if (nodeParent.ContainsKey(node))
                    {
                        nodeParent[node] = currentNode;
                    }
                    else
                    {
                        nodeParent.Add(node, currentNode);
                    }
                    stack.Push(node);
                }
            }
        }

        return startPosition;
    }
}
