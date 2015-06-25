using UnityEngine;
using System.Collections;

/*
 * Programmed By: Jay Chan Jr.
 * Date         : 11/MAR/2015
 * /

/***************************************************************************************
 * CLASS: CameraManager                                                                *
 * ------------------------------------------------------------------------------------*
 * The CameraManager class will be the one responsible for all camera behaviors during *
 * the game play scenes. Whenever a unit is focused on by the player, the camera will  *
 * be following that unit until a new unit is focused, or that is unit is destroyed.   *
 * A panning function is implemented as well in order for the player to pan across the *
 * game level. In addition, there's also a rudimentary zoom-in/zoom-out fuction also   *
 * available, but will be improved upon later.                                         *
 *-------------------------------------------------------------------------------------*
 * MEMBER METHODS:                                                                     *
 *                                                                                     *
 * Public / Non-Helper Methods --                                                      *
 *                                                                                     *
 * CameraPanning                                                                       *
 * -> Parameters: Vector3 mousePosOrigin                                               *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * CameraZooming                                                                       *
 * -> Parameters: float value                                                          *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * UnitFocus                                                                           *
 * -> Parameers: GameObject unitObj                                                    *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * SpawnIconFocus                                                                      *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * Private / Helper Methods --                                                         *
 *                                                                                     *
 * CameraFollowUnit                                                                    *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *
 *                                                                                     *
 * CameraFollowSpawn                                                                   *
 * -> Parameters: None                                                                 *
 * -> Returns: None                                                                    *                                                                    *
 **************************************************************************************/
public class CameraManager : MonoBehaviour 
{
	//Declaring data members

	private float camHalfWidth;

	private Vector3 minRec;
	private Vector3 maxRec;
	private float maxBoundx;

    private GameObject spawner;
    private Vector3 newPosition = new Vector3(0.0f,0.0f,-10.0f);
    private float panningSpeed = 2.0f;
    private Vector3 temp;
    private Vector3 pos;
	private Vector3 target;

    private bool buyState;
    private bool panCam;
	private GameObject focusedUnit;

    private Vector3 viewPortRectWorldPosLowerLeftPoint;
    private Vector3 viewPortRectWorldPosLowerRightPoint;
    private Vector3 viewPortRectWorldPosUpperRightPoint;

	public float camSpeed;
    public GameObject foreground;
    public bool perspectiveOn;
    public BoxCollider2D Bounds;
    public GameObject enemyObject;


    //CameraManager object instance which will be used to return its current instance
    private static CameraManager _instance;

    //This returns the object instance to wherever
    //this method is called. Basically, there will be
    //no need to have explicit references in the inspection
    //for this object in other scripts.
    public static CameraManager instance
    {
        get { return _instance; }
    }

    //---Setter and Getter for buyState;
    public bool isBuying
    {
        get { return buyState; }
        set { buyState = value; }
    }
	
    //--Setter and Getter for isPanning;
    public bool isPanning
    {
        get { return panCam; }
        set { panCam = value; }
    }

	// Use this for super initialization
	void Awake()
	{
        //Initializing _instance to our current instance of 
        //the CameraManager object
        _instance = this;
	}
			
	//Method: Start
    //Purpose -- Built in function in Unity that is called
    //when this class is first instantiated.
	void Start () 
	{

		//Setting the camera into ortographic mode
        if(perspectiveOn == true)
        {
            Camera.main.orthographic = true;
            camHalfWidth = Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
        }
        //Default setting is perspective camera
        else
        {
            //Viewport coordinates are relative to the camera, and normalized from 0 to 1
            //The next set of instructions converts the following points from viewport coordinates
            //to world coordinates.
            viewPortRectWorldPosLowerLeftPoint = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
            viewPortRectWorldPosLowerRightPoint = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10));
            viewPortRectWorldPosUpperRightPoint = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));

            //Getting the camera half vertical width
            camHalfWidth = Mathf.Abs(Vector3.Distance(viewPortRectWorldPosLowerLeftPoint, viewPortRectWorldPosLowerRightPoint));
            camHalfWidth = camHalfWidth/2.0f;
        }
		
		//Initializing the bounds of our level
		minRec = Bounds.bounds.min;
		maxRec = Bounds.bounds.max;
		maxBoundx = maxRec.x;

		if(GameManager.instance.numberOfCurrentUnit() >=1) 
        {
			focusedUnit = GameManager.instance.GetFocusedUnit();
		}
     	
        //Initializing buyState to false;
        buyState = false;
        panCam = false;
	}
	
    //Method: Update
    //Purpose -- Built in function in Unity that is called
    //once per frame.
	void Update () 
	{
        //Camera checks if the game isn't over yet.
        if(!GameManager.instance.missionComplete)
        {
            //This will keep changing the boundary of the camera in the 
            //x-position so it won't pan beyond the enemy object.
            maxRec.x = enemyObject.transform.position.x + 1.0f;
        }

        //Checks if the camera's x-position is more than the max boundary
        if(gameObject.transform.position.x + camHalfWidth > maxRec.x)
        {
            //Move it back inside if that's the case
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.01f, 
                                                        gameObject.transform.position.y, gameObject.transform.position.z);
        }

        //Making the camera move to the focused GameObject
        if(panCam == false && GameManager.instance.GetPausedState() == false)
        {
            //Lerp the camera to new position.
            CameraFollowUnit();
        }

        //If the player is in Buy mode, force the camera to follow the spawn icon instead
        if(GameManager.instance.GetPausedState() == true)
        {
            //Lerp the camera to the spawn icon
            CameraFollowSpawn();
        }
	}

	//Method: CameraPanning
	//Purpose -- This will make the camera pan around the world screen.
	public void CameraPanning(Vector3 mousePosOrigin)
	{
		//Transforms the mouse position from screen space to viewport space
		Vector3 mousePosNormalized = Camera.main.ScreenToViewportPoint(Input.mousePosition - mousePosOrigin);

		//This will be the new position of the mouse, thus will also be the camera's new position
		Vector3 mouseDestination = new Vector3(mousePosNormalized.x * panningSpeed, mousePosNormalized.y * panningSpeed, 0);

		temp = gameObject.transform.position + mouseDestination;
	
        //Clamp the camera movement in the x-position
		temp.x = Mathf.Clamp(temp.x, minRec.x + camHalfWidth, maxRec.x - camHalfWidth);

        if(perspectiveOn == false)
        {
            temp.y = Mathf.Clamp(temp.y, minRec.y + Camera.main.orthographicSize, maxRec.y - Camera.main.orthographicSize);
        }
        else
        {
            temp.y = Mathf.Clamp(temp.y, minRec.y + camHalfWidth, maxRec.y - camHalfWidth);
        }
	
		//This will Translate, or move, the camera's position to the mouse's position
		Camera.main.transform.position = temp;
	}

	//Method: CameraZooming
	//Purpose -- Handles zooming in and zooming out function.
	public void CameraZooming(float value)
	{
		//Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mousePosOrigin);
		//Vector3 mov = pos.y * zoomingSpeed * transform.forward;
		//transform.Translate(mov, Space.World);
		if (GetComponent<Camera>().orthographicSize < 5 && value > 0)
		{
			GetComponent<Camera>().orthographicSize += value;
		}
		if (GetComponent<Camera>().orthographicSize > 2.4 && value < 0)
		{
			GetComponent<Camera>().orthographicSize += value;
		}
	}


	//Method: CameraFollowUnit
    //Purpose -- As the name implies, this will tell the camera to follow
    //a specified game object
	private void CameraFollowUnit()
	{
        //Checks if there's a unit to focus
		if(focusedUnit != null)
		{
            //Set the new xy-position of the camera with the focused unit,
            //along with some offsets.
			newPosition.x = focusedUnit.transform.position.x + 4.0f;
			newPosition.y = focusedUnit.transform.position.y + 3.0f;

            //Clamps the x-position of the camera
            newPosition.x = Mathf.Clamp(newPosition.x, minRec.x + camHalfWidth, maxRec.x - camHalfWidth);

            //Check if camera is on perspective or ortho mode
            if(perspectiveOn == false)
            {
                //If ortho mode, use orthographic size to bound the camera in the y-direction
                newPosition.y = Mathf.Clamp(newPosition.y, minRec.y + Camera.main.orthographicSize, 
                                            maxRec.y - Camera.main.orthographicSize);
            }
            else
            {
                //Will be in perspective mode otherwise, and will utilize the camera half width
                //to restrict the camera in the y-direction
                newPosition.y = Mathf.Clamp(newPosition.y, minRec.y + camHalfWidth, maxRec.y - camHalfWidth);
            }

            //Finall moving the camera with a Lerp function
			Camera.main.transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * camSpeed);
		}
	}

	//========================================================
	//set position co camera follow the span.
    private void CameraFollowSpawn()
    {
        newPosition = new Vector3(spawner.transform.position.x +2.0f, spawner.transform.position.y + 2.0f, -10.0f);
		newPosition.x = Mathf.Clamp (newPosition.x, minRec.x + camHalfWidth, maxRec.x - camHalfWidth);
		newPosition.y = Mathf.Clamp (newPosition.y, minRec.y + Camera.main.orthographicSize, maxRec.y - Camera.main.orthographicSize);
		Camera.main.transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * camSpeed);
    }

	//Method: UnitFocus
	//Purpose -- UnitFocus will take in an argument of GameObject unitObj, this will tell the camera to then focus on that
	//GameObject. Essentially, the camera's transform will be changed to that of the transform of the GameObject.
	public void UnitFocus(GameObject unitObj)
	{
		focusedUnit = unitObj;
	}
	
    //Method: SpawnIconFocus
	//Purpose -- This method is called when the player goes into the buy state of the game. An icon will
    //be instantiated by the SpawnManager in which the CameraManager will then shift its focus
    //to the said icon, ignoring all other commands until it exits the buy state. 
    //Reminder -- UnitFocus also does the same job. Remove later.
    public void SpawnIconFocus(GameObject spawnObj)
    {
        spawner = spawnObj;
    }

}
