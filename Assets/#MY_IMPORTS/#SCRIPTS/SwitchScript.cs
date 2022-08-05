using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum SwitchType
{
	softSwitch,
	hardSwitch
}

public class SwitchScript : MonoBehaviour
{
	public bool switchOn = false;

	public SwitchType switchType;

	public UnityEvent onSwitchOn, onSwitchOff;

	public Animator[] bridgeTiles;

	

	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Player"))
		{
			StartCoroutine("InteractWithSwitch");
		}
	}

	IEnumerator InteractWithSwitch()
	{
		yield return new WaitForSeconds(0.1f);

		switch (switchType) 
		{
		case SwitchType.softSwitch:
			{
				
				switchOn = !switchOn;
				if (switchOn) 
				{
					Debug.Log("SOFT ON");
					AnimateBridgeTiles();
					onSwitchOn.Invoke();
				}
				else
				{
					Debug.Log("SOFT OFF");
					AnimateBridgeTiles();
					onSwitchOff.Invoke();
				}


				break;
			}
		case SwitchType.hardSwitch:
			{
				if (!CubeRollerScript.instance.upDownFlat && !CubeRollerScript.instance.leftRightFlat) 
				{
					switchOn = !switchOn;
					if (switchOn) 
					{
						Debug.Log("HARD ON");
						AnimateBridgeTiles();
						onSwitchOn.Invoke();
					}
					else
					{
						Debug.Log("HARD OFF");
						AnimateBridgeTiles();
						onSwitchOff.Invoke();
					}
				}

				break;
			}
		}
	}

	private void AnimateBridgeTiles()
	{
		if (switchOn) 
		{
			foreach(Animator a in bridgeTiles)
			{
				a.SetTrigger("On");
			}
		}
		else
		{
			foreach(Animator a in bridgeTiles)
			{
				a.SetTrigger("Off");
			}
		}
	}
}
