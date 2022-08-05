/*using UnityEngine;
using UnityEngine.UI;

/*public enum SwipeDirection
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8/*,
    UpLeft = 5,
    UpRight = 6,
    DownRight = 10,
    DownLeft = 9#2#
}#1#

public class SwipeManager : MonoBehaviour
{
    #region PUBLIC VARIABLES
    public static SwipeManager instance;


    public float swipeResistanceX = 100, swipeResistanceY = 100;
    
    public bool swipeControls = true;

    #endregion

    #region PRIVATE VARIABLES
    private Vector3 touchPosition;
    #endregion

    public SwipeDirection Direction { get; set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // if (!PlayerPrefs.HasKey("Controls Type"))
        // {
        //     SwipeControls(true);
        // }
        // if (PlayerPrefs.GetString("Controls Type") == "Swipe")
        // {
        //     SwipeControls(true);
        // }
        // else if(PlayerPrefs.GetString("Controls Type") == "Buttons")
        // {
        //     ButtonControls(true);
        // }
    }

    private void Update()
    {
        if (CubeRollerScript.instance.controlsActive)
        {
            if (swipeControls)
            {
                if (CubeRollerScript.instance.input)
                {
                    GetSwipeDirection();
                    RecordSwipes();
                }
            }
        }
    }
    
    void GetSwipeDirection()
    {
            if (Direction != SwipeDirection.None && IsSwipingOnScreen(SwipeDirection.Up))
            {
                Up();
            }
            else if (Direction != SwipeDirection.None && IsSwipingOnScreen(SwipeDirection.Down))
            {
                Down();
            }
            else if (Direction != SwipeDirection.None && IsSwipingOnScreen(SwipeDirection.Left))
            {
                Left();
            }
            else if (Direction != SwipeDirection.None && IsSwipingOnScreen(SwipeDirection.Right))
            {
                Right();
            }
    }


    void RecordSwipes()
    {
        Direction = SwipeDirection.None;

        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 deltaSwipe = touchPosition - Input.mousePosition;


            if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX)
            {
                //Swipe along X-axis
                Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }
            if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY)
            {
                //Swipe along Y-axis
                Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Up : SwipeDirection.Down;
            }
        }
    }

    public void Up()
    {
        if (CubeRollerScript.instance.input && CubeRollerScript.instance.controlsActive)
        {
            CubeRollerScript.instance.MoveCubeUp();
        }
    }
    public void Down()
    {
        if (CubeRollerScript.instance.input && CubeRollerScript.instance.controlsActive)
        {
            CubeRollerScript.instance.MoveCubeDown();
        }
    }
    public void Left()
    {
        if (CubeRollerScript.instance.input && CubeRollerScript.instance.controlsActive)
        {
            CubeRollerScript.instance.MoveCubeLeft();
        }
    }
    public void Right()
    {
        if (CubeRollerScript.instance.input && CubeRollerScript.instance.controlsActive)
        {
            CubeRollerScript.instance.MoveCubeRight();
        }
    }
    
    public bool IsSwipingOnScreen(SwipeDirection dir)
    {
        return (Direction & dir) == dir;
    }
}*/