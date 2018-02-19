using UnityEngine;

public class CameraMovementFPS : MonoBehaviour {

    // Use this for initialization
    Material renderingMaterial;
    Transform caughtObjectTransform;

	void Start () {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f, 0.0f, Input.GetAxis("Vertical") * Time.deltaTime * 10.0f, Space.Self);
        transform.Rotate(0f, Input.GetAxis("Mouse X") * Time.deltaTime * 30f, 0.0f, Space.World);
        transform.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * 30.0f, 0.0f, 0.0f, Space.Self);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newcube = GameObject.Instantiate(Resources.Load("OctCube")) as GameObject;
            newcube.transform.position = this.transform.position + transform.forward * 6;
        }

        RaycastHit ray;
        if(Physics.Raycast(transform.position, transform.forward , out ray, 100.0f))
        {
            if(ray.collider.tag == "OctCube")
            {
                 if(renderingMaterial != null)
                     renderingMaterial.color = Color.white;

                GameObject caughtObject = ray.collider.gameObject;
                Rigidbody caughtRigidComp = caughtObject.GetComponent<Rigidbody>();
                renderingMaterial = caughtObject.GetComponent<Renderer>().material;
                renderingMaterial.color = Color.red;

                if(Input.GetMouseButtonDown(1))
                {
                    caughtRigidComp.isKinematic = true;
                    caughtObjectTransform = caughtObject.transform;
                    caughtObjectTransform.parent = this.transform;
                }

                if(Input.GetMouseButtonUp(1))
                {
                    caughtRigidComp.isKinematic = false;
                    caughtObjectTransform = caughtObject.transform;
                    caughtObjectTransform.parent = null;
                }
                // TODO: delete the cube that is created
                if (Input.GetKeyUp(KeyCode.E))
                {
                    Destroy(caughtObject.gameObject);
                }
                if (Input.GetKeyUp(KeyCode.R))
                {
                    caughtRigidComp.AddForce(transform.forward * 100f);
                }

            }
        }
        else
        {
            if(renderingMaterial != null)
            {
                renderingMaterial.color = Color.white;
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
