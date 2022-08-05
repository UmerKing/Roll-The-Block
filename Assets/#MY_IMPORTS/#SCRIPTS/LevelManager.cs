using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;
	
	public Level[] levelPrefabs;
	public GameObject player;
	public Transform playerMesh;
	public Transform[] playerSpawnPoints;
	public GameObject slamParticles, confettiParticles;


    void Awake()
    {
	    if (instance == null)
	    {
		    instance = this;
	    }
	    
	    
	    
	    Instantiate(levelPrefabs[GameManager.instance.selectedLevel].levelPrefab);
	    player.transform.position = playerSpawnPoints[GameManager.instance.selectedLevel].position;
	    playerMesh.position = new Vector3(playerMesh.position.x, 10, playerMesh.position.z);
	    
	    playerMesh.DOLocalMove(new Vector3(0.5f, 0, 0), 0.2f).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(
		    () =>
		    {
			    CubeRollerScript.instance.controlsActive = true;
			    slamParticles.SetActive(true);
			    slamParticles.transform.SetParent(null);
		    });

		player.SetActive(true);
		LoadCurrentBestMoves();
    }

    void LoadCurrentBestMoves()
    {
	    for (int i = 1; i < levelPrefabs.Length; i++)
	    {
		    levelPrefabs[i].levelCurrentBest = PlayerPrefs.GetInt("Level " + i + " Best");
	    }
    }
}

[System.Serializable]
public class Level
{
	public GameObject levelPrefab;
	public int levelTarget;
	public int levelCurrentBest;
}
