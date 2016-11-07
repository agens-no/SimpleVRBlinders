using UnityEngine;

[System.Serializable]
public struct GettoEagle
{
    public float velocity;
    public float tiltTurnFactor;
    public float radiusOfTheSpot;
    public float tiltAngleSpotFactor;
    public float maxOfBlenderSpotX;
    public float maxOfBlenderSpotY;
    public float deAccForBlinds;
    public float minRadiusForBlinds;
    public float radiusBlindsSpeedForNearbyObjects;
    public float proximityToStuffSpeedOfBlinderMovement;
    public float sidewaysRayLength;
    public float updownsRayLength;
}

public class TakeFlight : MonoBehaviour
{

    public GettoEagle gettoEagle;
    public Transform ourDirectionThing;
    public Renderer blinds;
    public Vector4 spotOfTheLight;
    public float currentRadiusOfSpot;

    // Use this for initialization
	void Start () {

	}


	// Update is called once per frame
	void Update ()
	{

	    Vector3 forward = ourDirectionThing.rotation * Vector3.forward;

	    float tiltAngles = ourDirectionThing.localRotation.eulerAngles.z;
	    tiltAngles= (tiltAngles> 180.0f) ? tiltAngles- 360.0f : tiltAngles;

	    transform.Rotate(Vector3.up,-tiltAngles*Time.deltaTime*gettoEagle.tiltTurnFactor);
	    transform.Translate(forward.normalized*gettoEagle.velocity*Time.deltaTime,Space.World);

	    //adjust blinders based on head tilt
	    spotOfTheLight.x += tiltAngles * gettoEagle.tiltAngleSpotFactor * Time.deltaTime;

	    //lerp towards no/low blinders
	    spotOfTheLight = Vector4.Lerp(spotOfTheLight, Vector4.zero, Time.deltaTime*gettoEagle.deAccForBlinds);
	    currentRadiusOfSpot = Mathf.Lerp(currentRadiusOfSpot, gettoEagle.radiusOfTheSpot,
	                                       Time.deltaTime * gettoEagle.deAccForBlinds);

	    float sideWindLeft = 0f;
	    float sideWindRight = 0f;
	    float sideWindUp= 0f;
	    float sideWindDown = 0f;

	    Vector3 downAdjusted = new Vector3(forward.x,forward.y-2.0f,forward.z).normalized;
	    Vector3 upAdjusted = new Vector3(forward.x,forward.y +2.0f,forward.z).normalized;


	    Debug.DrawRay(transform.position,downAdjusted*gettoEagle.updownsRayLength,Color.black);
	    Debug.DrawRay(transform.position,upAdjusted*gettoEagle.updownsRayLength,Color.blue);
	    Debug.DrawRay(transform.position,Quaternion.AngleAxis(30.0f,Vector3.up)*forward*gettoEagle.sidewaysRayLength,Color.yellow);
	    Debug.DrawRay(transform.position,Quaternion.AngleAxis(-30.0f,Vector3.up)*forward*gettoEagle.sidewaysRayLength,Color.magenta);


	    //here is where you should check distance to object and also calculate relative speed difference
	    //if you want that to be taken into consideration.
	    RaycastHit hitta;
	    if (Physics.Raycast(
	        transform.position,
	        Quaternion.AngleAxis(30.0f, Vector3.up) * forward,
	        out hitta, gettoEagle.sidewaysRayLength))
	    {
	        sideWindLeft = 1.0f;
	    }

	    if (Physics.Raycast(
	        transform.position,
	        Quaternion.AngleAxis(-30.0f, Vector3.up) * forward,
	        out hitta, gettoEagle.sidewaysRayLength))
	    {
	        sideWindRight = 1.0f;
	    }

	    if (Physics.Raycast(
	        transform.position,
	        upAdjusted,
	        out hitta, gettoEagle.updownsRayLength))
	    {
	        sideWindUp = 1.0f;
	    }

	    if (Physics.Raycast(
	        transform.position,
	        downAdjusted,
	        out hitta, gettoEagle.updownsRayLength))
	    {
	        sideWindDown = 1.0f;
	    }


	    //influnce spot of the light by proximity to objects
	    spotOfTheLight.x -= sideWindLeft*Time.deltaTime*gettoEagle.proximityToStuffSpeedOfBlinderMovement;
	    spotOfTheLight.x += sideWindRight*Time.deltaTime*gettoEagle.proximityToStuffSpeedOfBlinderMovement;

	    spotOfTheLight.y += sideWindDown*Time.deltaTime*gettoEagle.proximityToStuffSpeedOfBlinderMovement;
	    spotOfTheLight.y -= sideWindUp*Time.deltaTime*gettoEagle.proximityToStuffSpeedOfBlinderMovement;

	    //if we are getting "banged" from both sides / all sides, shrink radius of focus spot.
	    float amountOfRadiusDiminisherSideWays = Mathf.Clamp(sideWindLeft + sideWindRight - 1f,0.0f,1.0f)*gettoEagle.radiusBlindsSpeedForNearbyObjects;
	    float amountOfRadiusDiminisherUpDown = Mathf.Clamp(sideWindUp+ sideWindDown- 1f,0.0f,1.0f)*gettoEagle.radiusBlindsSpeedForNearbyObjects;

	    currentRadiusOfSpot -= amountOfRadiusDiminisherSideWays*Time.deltaTime;
	    currentRadiusOfSpot -= amountOfRadiusDiminisherUpDown*Time.deltaTime;
	    currentRadiusOfSpot = Mathf.Clamp(currentRadiusOfSpot, gettoEagle.minRadiusForBlinds,gettoEagle.radiusOfTheSpot);

	    spotOfTheLight.y = Mathf.Clamp(spotOfTheLight.y,-gettoEagle.maxOfBlenderSpotY,gettoEagle.maxOfBlenderSpotY);
	    spotOfTheLight.x = Mathf.Clamp(spotOfTheLight.x,-gettoEagle.maxOfBlenderSpotX,gettoEagle.maxOfBlenderSpotX);

	    //transfer information to shader
	    blinds.material.SetVector("_FocusSpot",spotOfTheLight);
	    blinds.material.SetFloat("_Radius",currentRadiusOfSpot);

	}

}
