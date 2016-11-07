using UnityEngine;
using System.Collections;

public class FakeHeadController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public float horiAxx;
    public float vertAxx;
    public float tiltAxx;

    public float turningSpeed;

    // Update is called once per frame
	void Update ()
	{

	    horiAxx = Input.GetAxis("Horizontal");
	    vertAxx = Input.GetAxis("Vertical");
	    tiltAxx = -(Input.GetAxis("lTrigger")*0.5f +0.5f);
	    tiltAxx += (Input.GetAxis("rTrigger")*0.5f +0.5f);


	    transform.Rotate(Vector3.forward,-tiltAxx*turningSpeed*Time.deltaTime);
	    transform.Rotate(Vector3.up,horiAxx*turningSpeed*Time.deltaTime,Space.World);
	    transform.Rotate(Vector3.right,vertAxx*turningSpeed*Time.deltaTime);

	}
}
