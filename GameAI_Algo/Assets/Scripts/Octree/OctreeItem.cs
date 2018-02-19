using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeItem : MonoBehaviour {

    public List<OctNode> octreeNodes = new List<OctNode>();
    private Vector3 previousPosition;
	// Use this for initialization
	void Start () {
        previousPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(transform.position != previousPosition)
        {
            OctNode.octTreeRootNode.ProcessItem(this);
            previousPosition = transform.position;
        }
	}
}
