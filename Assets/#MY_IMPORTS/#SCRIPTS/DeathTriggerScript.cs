using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public enum AlongAxis
{
    alongX,
    alongXN,
    alongZ,
    alongZN
}

public class DeathTriggerScript : MonoBehaviour
{
    public Transform closestPivotPoint, centerPivot;
    public AlongAxis axis;
    [HideInInspector]public GameObject player;
    public float speed = 0.1f;
    public string direction;
    
    void Start()
    {
        player = FindObjectOfType<CubeRollerScript>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Death"))
        {
            Debug.Log("Death");
            CubeRollerScript.instance.isDead = true;
            if (SoundManager.instance != null)
            {
                SoundManager.instance.death.Play();
                SoundManager.instance.musicAudioSource.DOFade(0, 0.3f);
            }
            
            GameplayManager.instance.FailLevel();
            player.GetComponent<CubeRollerScript>().onDeath.Invoke();
            
            if (CubeRollerScript.instance.t1 == null)
            {
                CubeRollerScript.instance.t1 = this;
            }
            else
            {
                CubeRollerScript.instance.t2 = this;
            }

            StartCoroutine(CubeRollerScript.instance.DecidePivot());
        }
    }

    public void RotateCubeAroundFull()
    {
        StartCoroutine(RotateCubeFull(closestPivotPoint, axis));
    }
    public void RotateCubeAroundCenter()
    {
        StartCoroutine(RotateCubeHalf(centerPivot, axis));
    }
    
    public IEnumerator RotateCubeFull(Transform pivotPoint, AlongAxis alongAxis)
    {
        foreach (GameObject g in player.GetComponent<CubeRollerScript>().allTriggers)
        {
            g.GetComponent<BoxCollider>().enabled = false;
        }
        
        switch (alongAxis)
        {
            case AlongAxis.alongX:
            {
                
                
                for (int i = 0; i < (90 / 9); i++)
                {
                    yield return new WaitForSeconds(speed);
                }
                
                break;
            }
        }
    }
    public IEnumerator RotateCubeHalf(Transform pivotPoint, AlongAxis alongAxis)
    {
        yield return new WaitForSeconds(0.1f);
        
        foreach (GameObject g in player.GetComponent<CubeRollerScript>().allTriggers)
        {
            g.GetComponent<BoxCollider>().enabled = false;
        }
        
        switch (alongAxis)
        {
            case AlongAxis.alongX:
            {
                
                
                if (direction == "P")
                {
                    for (int i = 0; i < (90 / 9); i++)
                    {
                        player.transform.RotateAround(pivotPoint.transform.position, pivotPoint.right, 9);
                        yield return new WaitForSeconds(speed);
                    }
                }
                
                if (direction == "N")
                {
                    for (int i = 0; i < (90 / 9); i++)
                    {
                        player.transform.RotateAround(pivotPoint.transform.position, -pivotPoint.right, 9);
                        yield return new WaitForSeconds(speed);
                    }
                }
                
                break;
            }
        }
    }
}
