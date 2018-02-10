using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class AgentBehavior : MonoBehaviour {

    IDictionary<Node, Node> nodeParent = new Dictionary<Node, Node>();
    public Node result;

    public Material BFSmaterial;
    public Material DFSmaterial;
    public Material AStarmaterial;



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

    public void ShowAStar()
    {
        Node start = GridBase.GetInstance().NodeFromWorldPosition(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position);// GridBase.GetInstance().grid[0, 0]; //
        Node goal = GridBase.GetInstance().NodeFromWorldPosition(GameObject.FindGameObjectWithTag("Goal").gameObject.transform.position);//GridBase.GetInstance().grid[5, 5];//

        result = GetShortestPathAStar(start, goal);
        Node cur = result;

        while (cur != start)
        {
            cur.tileMeshRenderer.material = AStarmaterial;
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

    Node GetShortestPathAStar(Node startPosition, Node GoalNode)
    {
        float time = Time.realtimeSinceStartup;

        List<Node> validNodeList = GridBase.GetInstance().GetAllWalkableNodes();

        IDictionary<Node, int> heuristicScore = new Dictionary<Node, int>();

        IDictionary<Node, int> distanceFromStart = new Dictionary<Node, int>();

        foreach(Node i_node in validNodeList)
        {
            heuristicScore.Add(new KeyValuePair<Node, int>(i_node, int.MaxValue));
            distanceFromStart.Add(new KeyValuePair<Node, int>(i_node, int.MaxValue));
        }

        heuristicScore[startPosition] = EuclideanDistance(startPosition, GoalNode);
        distanceFromStart[startPosition] = 0;

        HashSet<Node> exploredNodes = new HashSet<Node>();

        SimplePriorityQueue<Node, int> priorityQueue = new SimplePriorityQueue<Node, int>();
        priorityQueue.Enqueue(startPosition, heuristicScore[startPosition]);

        while(priorityQueue.Count > 0)
        {
            Node currentNode = priorityQueue.Dequeue();

            if (currentNode == GoalNode)
                return currentNode;

            exploredNodes.Add(currentNode);

            List<Node> adjacentWalkableNodes = GridBase.GetInstance().GetWalkableAdjacentNodes(currentNode);

            foreach(Node i_node in adjacentWalkableNodes)
            {
                if (exploredNodes.Contains(i_node))
                    continue;

                int currentNodeScore = distanceFromStart[currentNode] + 1;

                if(!priorityQueue.Contains(i_node))
                {
                    priorityQueue.Enqueue(i_node, heuristicScore[i_node]);
                }
                else if( currentNodeScore > distanceFromStart[i_node])
                {
                    continue;
                }

                if (nodeParent.ContainsKey(i_node))
                {
                    nodeParent[i_node] = currentNode;
                }
                else
                {
                    nodeParent.Add(i_node, currentNode);
                }

                distanceFromStart[i_node] = currentNodeScore;
                heuristicScore[i_node] = distanceFromStart[i_node];//+ EuclideanDistance(currentNode, GoalNode);
            }
        }

        return startPosition;
    }

    int EuclideanDistance(Node start, Node end)
    {
        int result;

        result = (int)Mathf.Sqrt(
                                Mathf.Pow(start.nodePositionX - end.nodePositionX, 2) +
                                //Mathf.Pow(start.nodePositionY - end.nodePositionX, 2)+
                                Mathf.Pow(start.nodePositionZ - end.nodePositionZ, 2));

        return result;
    }
}
