using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum SwipeDirection
{
	None = 0,
	Left = 1,
	Right = 2,
	Up = 4,
	Down = 8/*,
    UpLeft = 5,
    UpRight = 6,
    DownRight = 10,
    DownLeft = 9*/
}

public class CubeRollerScript : MonoBehaviour
{
	public static CubeRollerScript instance;

	public bool controlsActive = true;

	public GameObject player;

	public GameObject center;
	public GameObject up;
	public GameObject down;
	public GameObject left;
	public GameObject right;

	public int step = 9;
	public float speed = 0.01f;
	public float swipeDelay = 0.3f;

	[HideInInspector] public bool input = true;

	[HideInInspector] public bool flat;

	[HideInInspector] public bool upDownFlat, leftRightFlat;

	public GameObject sensorTop, sensorBottom;

	private bool checkSensors;

	[Header("FINISH")]
	public Animator playerAnimator;

	[Header("DEATH")]
	[HideInInspector] public bool isDead;
	public DeathTriggerScript t1, t2;
	[HideInInspector] public bool decided;
	public GameObject[] allTriggers;

	public UnityEvent onDeath;

	[Header("MOVES COUNT")]
	public int movesCount;
	public TMP_Text movesCountText;
	
	[Header("STARS")]
	public int starsCollected;

	public GameObject[] starsFills;

	[Header("SKINS")] 
	public Material playerMat;

	//public int selectedSkin;
	//public Texture[] skins;

	#region SWIPE MANAGER

	public SwipeDirection Direction { get; set; }
	public bool swipeControls = true;
	private Vector3 touchPosition;
	public float swipeResistanceX = 100, swipeResistanceY = 100;

	#endregion

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

		center.transform.SetParent(null);
		//SelectSkin();
	}

	public IEnumerator DecidePivot()
	{
		yield return  new WaitForEndOfFrame();
		if (!decided)
		{
			decided = true;

			if (t1 != null && t2 != null)
			{
				t1.RotateCubeAroundFull();
			}
			else if (t2 == null)
			{
				t1.RotateCubeAroundCenter();
			}
		}
	}

	public void DeadPlayer()
	{
		isDead = true;
			
		transform.DOMove(new Vector3(transform.position.x, -20, transform.position.z), 0.7f).SetEase(Ease.Linear).SetDelay(0.35f);
		
		/*if (!isDead)
		{
			isDead = true;
			
			transform.DOMove(new Vector3(transform.position.x, -20, transform.position.z), 0.7f).SetEase(Ease.Linear)
				.SetDelay(0.35f);
		}*/
	}
	
	void Update()
	{
		if (controlsActive) 
		{
			if (swipeControls)
			{
				if (input)
				{
					GetSwipeDirection();
					RecordSwipes();
				}
			}
			
			if (input) 
			{
				if (Input.GetKey(KeyCode.UpArrow))
				{
					UpButton();
				}
				else if (Input.GetKey(KeyCode.DownArrow))
				{
					DownButton();
				}
				else if (Input.GetKey(KeyCode.LeftArrow))
				{
					LeftButton();
				}
				else if (Input.GetKey(KeyCode.RightArrow))
				{
					RightButton();
				}
			}
		}
	}
	

	public void UpButton()
	{
		if (input && controlsActive)
		{
			if (SoundManager.instance != null)
			{
				SoundManager.instance.swipeOrMoveClick.Play();
				SoundManager.instance.move.Play();
			}
			StartCoroutine(MoveUp());
			input = false;
		}
	}
	public void DownButton()
	{
		if (input && controlsActive)
		{
			if (SoundManager.instance != null)
			{
				SoundManager.instance.swipeOrMoveClick.Play();
				SoundManager.instance.move.Play();
			}
			StartCoroutine(MoveDown());
			input = false;
		}
	}
	public void LeftButton()
	{
		if (input && controlsActive)
		{
			if (SoundManager.instance != null)
			{
				SoundManager.instance.swipeOrMoveClick.Play();
				SoundManager.instance.move.Play();
			}
			StartCoroutine(MoveLeft());
			input = false;
		}
	}
	public void RightButton()
	{
		if (input && controlsActive)
		{
			if (SoundManager.instance != null)
			{
				SoundManager.instance.swipeOrMoveClick.Play();
				SoundManager.instance.move.Play();
			}
			StartCoroutine(MoveRight());
			input = false;
		}
	}

	void FixedUpdate()
	{
		if(checkSensors)
		{
			RaycastHit hit;
			if (Physics.Raycast(sensorTop.transform.position, sensorTop.transform.forward, out hit, 0.5f))
			{
				if (!isDead && hit.collider.gameObject.tag.Equals("Finish"))
				{
					if (SoundManager.instance != null)
					{
						SoundManager.instance.finish.Play();
						SoundManager.instance.finish2.Play();
						SoundManager.instance.win.Play();
					}
					
					controlsActive = false;
					playerAnimator.enabled = true;
					playerAnimator.SetTrigger("Bottom To Top");
					StartCoroutine(SpawnConfetti());
					GameplayManager.instance.CompleteLevel();
				}
				else if (hit.collider.gameObject.tag.Equals("Death"))
				{
					isDead = true;
					if (SoundManager.instance != null)
					{
						SoundManager.instance.death.Play();
						SoundManager.instance.musicAudioSource.DOFade(0, 0.3f);
					}
					
					foreach (GameObject g in allTriggers)
					{
						g.GetComponent<BoxCollider>().enabled = false;
					}
					GameplayManager.instance.FailLevel();
					onDeath.Invoke();
				}
				checkSensors = false;
			}

			RaycastHit hit2;
			if (Physics.Raycast(sensorBottom.transform.position, sensorBottom.transform.forward, out hit2, 0.5f))
			{
				if (!isDead && hit2.collider.gameObject.tag.Equals("Finish"))
				{
					if (SoundManager.instance != null)
					{
						SoundManager.instance.finish.Play();
						SoundManager.instance.finish2.Play();
						SoundManager.instance.win.Play();
					}
					
					controlsActive = false;
					playerAnimator.enabled = true;
					playerAnimator.SetTrigger("Top To Bottom");
					StartCoroutine(SpawnConfetti());
					GameplayManager.instance.CompleteLevel();
				}
				else if (hit2.collider.gameObject.tag.Equals("Death"))
				{
					isDead = true;
					if (SoundManager.instance != null)
					{
						SoundManager.instance.death.Play();
						SoundManager.instance.musicAudioSource.DOFade(0, 0.3f);
					}
					
					foreach (GameObject g in allTriggers)
					{
						g.GetComponent<BoxCollider>().enabled = false;
					}
					
					GameplayManager.instance.FailLevel();
					onDeath.Invoke();
				}
				checkSensors = false;
			}
		}
	}

	IEnumerator SpawnConfetti()
	{
		yield return new WaitForSecondsRealtime(0.3f);
		LevelManager.instance.confettiParticles.transform.position = player.transform.position;
		LevelManager.instance.confettiParticles.SetActive(true);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Star")) 
		{
			if (SoundManager.instance != null)
			{
				SoundManager.instance.starPickup.Play();
			}
			other.gameObject.SetActive(false);
			UpdateStars();
		}
	}

	void UpdateStars()
	{
		starsCollected++;

		switch (starsCollected)
		{
			case 1:
			{
				starsFills[0].SetActive(true);
				break;
			}
			case 2:
			{
				starsFills[1].SetActive(true);
				break;
			}
			case 3:
			{
				starsFills[2].SetActive(true);
				break;
			}
		}
	}

    IEnumerator MoveUp()
    {
        for (int i = 0; i < (90/step); i++)
		{
			player.transform.RotateAround(up.transform.position, Vector3.right, step);
			yield return new WaitForSeconds(speed);
        }


		if (!leftRightFlat)
		{
			upDownFlat = !upDownFlat;
		}

		if (!leftRightFlat)
		{
			flat = !flat;

			if (flat) 
			{
				up.transform.position = new Vector3(up.transform.position.x, up.transform.position.y, up.transform.position.z + 0.5f);
				down.transform.position = new Vector3(down.transform.position.x, down.transform.position.y, down.transform.position.z - 0.5f);
			}
			else
			{
				up.transform.position = new Vector3(up.transform.position.x, up.transform.position.y, up.transform.position.z - 0.5f);
				down.transform.position = new Vector3(down.transform.position.x, down.transform.position.y, down.transform.position.z + 0.5f);
			}
		}
		center.transform.position = new Vector3(player.transform.position.x, center.transform.position.y, player.transform.position.z);

		checkSensors = true;
		movesCount++;
		movesCountText.text = movesCount.ToString();
		yield return new WaitForSecondsRealtime(swipeDelay);
		input = true;
    }
    
    IEnumerator MoveDown()
	{
		for (int i = 0; i < (90/step); i++)
		{
			player.transform.RotateAround(down.transform.position, Vector3.left, step);
			yield return new WaitForSeconds(speed);
		}



		if (!leftRightFlat)
		{
			upDownFlat = !upDownFlat;
		}

		if (!leftRightFlat)
		{
			flat = !flat;

			if (flat) 
			{
				down.transform.position = new Vector3(down.transform.position.x, down.transform.position.y, down.transform.position.z - 0.5f);
				up.transform.position = new Vector3(up.transform.position.x, up.transform.position.y, up.transform.position.z + 0.5f);
			}
			else
			{
				down.transform.position = new Vector3(down.transform.position.x, down.transform.position.y, down.transform.position.z + 0.5f);
				up.transform.position = new Vector3(up.transform.position.x, up.transform.position.y, up.transform.position.z - 0.5f);
			}
		}
		center.transform.position = new Vector3(player.transform.position.x, center.transform.position.y, player.transform.position.z);

		checkSensors = true;
		
		movesCount++;
		movesCountText.text = movesCount.ToString();
		
		yield return new WaitForSecondsRealtime(swipeDelay);
		input = true;
	}

    IEnumerator MoveLeft()
	{
		for (int i = 0; i < (90/step); i++)
		{
			player.transform.RotateAround(left.transform.position, Vector3.forward, step);
			yield return new WaitForSeconds(speed);
		}



		if (!upDownFlat) 
		{
			leftRightFlat = !leftRightFlat;
		}

		if (!upDownFlat) 
		{
			flat = !flat;

			if (flat) 
			{
				left.transform.position = new Vector3(left.transform.position.x - 0.5f, left.transform.position.y, left.transform.position.z);
				right.transform.position = new Vector3(right.transform.position.x + 0.5f, right.transform.position.y, right.transform.position.z);
			}
			else
			{
				left.transform.position = new Vector3(left.transform.position.x + 0.5f, left.transform.position.y, left.transform.position.z);
				right.transform.position = new Vector3(right.transform.position.x - 0.5f, right.transform.position.y, right.transform.position.z);
			}
		}

		center.transform.position = new Vector3(player.transform.position.x, center.transform.position.y, player.transform.position.z);

		checkSensors = true;
		
		movesCount++;
		movesCountText.text = movesCount.ToString();
		
		yield return new WaitForSecondsRealtime(swipeDelay);
		input = true;
	}

    IEnumerator MoveRight()
	{
		for (int i = 0; i < (90/step); i++)
		{
			player.transform.RotateAround(right.transform.position, Vector3.back, step);
			yield return new WaitForSeconds(speed);
		}



		if (!upDownFlat) 
		{
			leftRightFlat = !leftRightFlat;
		}


		if (!upDownFlat) 
		{
			flat = !flat;

			if (flat) 
			{
				right.transform.position = new Vector3(right.transform.position.x + 0.5f, right.transform.position.y, right.transform.position.z);
				left.transform.position = new Vector3(left.transform.position.x - 0.5f, left.transform.position.y, left.transform.position.z);
			}
			else
			{
				right.transform.position = new Vector3(right.transform.position.x - 0.5f, right.transform.position.y, right.transform.position.z);
				left.transform.position = new Vector3(left.transform.position.x + 0.5f, left.transform.position.y, left.transform.position.z);
			}
		}
		center.transform.position = new Vector3(player.transform.position.x, center.transform.position.y, player.transform.position.z);

		checkSensors = true;
		
		movesCount++;
		movesCountText.text = movesCount.ToString();
		
		yield return new WaitForSecondsRealtime(swipeDelay);
		input = true;
	}

    public void MoveCubeUp()
    {
	    StartCoroutine(MoveUp());
	    input = false;
    }
    public void MoveCubeDown()
    {
	    StartCoroutine(MoveDown());
	    input = false;
    }
    public void MoveCubeLeft()
    {
	    StartCoroutine(MoveLeft());
	    input = false;
    }
    public void MoveCubeRight()
    {
	    StartCoroutine(MoveRight());
	    input = false;
    }

	public void EnableControls()
	{
		StartCoroutine(GameplayManager.instance.EnableControls());

	}
	public void DisableControls()
	{
		controlsActive = false;
	}

	/*public void SelectSkin()
	{
		playerMat.mainTexture = skins[selectedSkin];
	}*/

	#region SWIPE MANAGER

	void GetSwipeDirection()
    {
	    if (controlsActive)
	    {
		    if (Direction != SwipeDirection.None && IsSwipingOnScreen(SwipeDirection.Up))
		    {
			    if (SoundManager.instance != null)
			    {
				    SoundManager.instance.move.Play();
				    SoundManager.instance.swipeOrMoveClick.Play();
			    }
			    Up();
		    }
		    else if (Direction != SwipeDirection.None && IsSwipingOnScreen(SwipeDirection.Down))
		    {
			    if (SoundManager.instance != null)
			    {
				    SoundManager.instance.move.Play();
				    SoundManager.instance.swipeOrMoveClick.Play();
			    }
			    Down();
		    }
		    else if (Direction != SwipeDirection.None && IsSwipingOnScreen(SwipeDirection.Left))
		    {
			    if (SoundManager.instance != null)
			    {
				    SoundManager.instance.move.Play();
				    SoundManager.instance.swipeOrMoveClick.Play();
			    }
			    Left();
		    }
		    else if (Direction != SwipeDirection.None && IsSwipingOnScreen(SwipeDirection.Right))
		    {
			    if (SoundManager.instance != null)
			    {
				    SoundManager.instance.move.Play();
				    SoundManager.instance.swipeOrMoveClick.Play();
			    }
			    Right();
		    }
	    }
    }


    void RecordSwipes()
    {
	    if (controlsActive)
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
    }

    public void Up()
    {
        if (input && controlsActive)
        {
            MoveCubeUp();
        }
    }
    public void Down()
    {
        if (input && controlsActive)
        {
            MoveCubeDown();
        }
    }
    public void Left()
    {
        if (input && controlsActive)
        {
            MoveCubeLeft();
        }
    }
    public void Right()
    {
        if (input && controlsActive)
        {
            MoveCubeRight();
        }
    }
    
    public bool IsSwipingOnScreen(SwipeDirection dir)
    {
        return (Direction & dir) == dir;
    }

	#endregion
}
