using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    
    //Gameplay panel
    public DOTweenAnimation gameplayPanel;
    public TMP_Text gameplayPanelTargetMoves;
    
    //Pause panel
    public GameObject pausePanel;
    public bool isPaused;
    public TMP_Text pausePanelMoves;
    
    //Complete panel
    public GameObject completePanel;
    public TMP_Text completePanelMoves;
    public TMP_Text completePanelBestMoves;
    public TMP_Text completePanelTargetMoves;
    public GameObject[] completePanelStars;
    public TMP_Text bestTagText;
    
    //Fail panel
    public GameObject failPanel;

    public TMP_Text levelNumberText;

    public GameObject loadingPanel;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        once = false;
        Time.timeScale = 1;
        levelNumberText.text = "Level " + GameManager.instance.selectedLevel;
        gameplayPanelTargetMoves.text = "TARGET : " + LevelManager.instance.levelPrefabs[GameManager.instance.selectedLevel].levelTarget;
        
        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.HideSmallBanner();
        }
    }

    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseLevel()
    {
        isPaused = !isPaused;
        
        //PAUSE
        if (isPaused)
        {
            CubeRollerScript.instance.controlsActive = false;
            
            failPanel.SetActive(false);
            completePanel.SetActive(false);

            pausePanel.SetActive(true);
            pausePanelMoves.text = CubeRollerScript.instance.movesCount.ToString();
            Time.timeScale = 0;
            
            if (GoogleMobileAdsManager.Instance != null)
            {
                //GoogleMobileAdsManager.Instance.ShowSmallBanner();
                
                GoogleMobileAdsManager.Instance.ShowInterstitial();
            }
        }
        
        //UNPAUSE
        else
        {
            failPanel.SetActive(false);
            completePanel.SetActive(false);
            pausePanel.SetActive(false);
            
            Time.timeScale = 1;
            StartCoroutine(EnableControls());
            
            if (GoogleMobileAdsManager.Instance != null)
            {
                GoogleMobileAdsManager.Instance.HideSmallBanner();
            }
        }
    }

    public IEnumerator EnableControls()
    {
        yield return new WaitForSeconds(0.2f);
        CubeRollerScript.instance.controlsActive = true;
    }
    public void CompleteLevel()
    {
        int starGot = Random.Range(0, 4);
        CubeRollerScript.instance.controlsActive = false;

        if (PlayerPrefs.GetInt("Level Stars " + GameManager.instance.selectedLevel) < CubeRollerScript.instance.starsCollected || PlayerPrefs.GetInt("Level Stars " + GameManager.instance.selectedLevel) == 0)
        {
            PlayerPrefs.SetInt(("Level Stars " + GameManager.instance.selectedLevel), CubeRollerScript.instance.starsCollected);

            for (int i = 0; i < CubeRollerScript.instance.starsCollected; i++)
            {
                completePanelStars[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < CubeRollerScript.instance.starsCollected; i++)
            {
                completePanelStars[i].SetActive(true);
            }
        }

        if (!PlayerPrefs.HasKey("Level " + (GameManager.instance.selectedLevel + 1)) && GameManager.instance.selectedLevel < LevelManager.instance.levelPrefabs.Length - 1)
        {
            PlayerPrefs.SetInt("Level " + (GameManager.instance.selectedLevel + 1), 1);
        }
        else
        {
            PlayerPrefs.SetInt("All Levels Completed", 1);
        }

        if ((GameManager.instance.selectedLevel + 1) % 9 == 0 && PlayerPrefs.GetInt("Current Page") <= (GameManager.instance.selectedLevel + 1)/9)
        {
            PlayerPrefs.SetInt("Current Page", PlayerPrefs.GetInt("Current Page") + 1);
        }
        
        if (PlayerPrefs.GetInt("Current Level") < GameManager.instance.selectedLevel + 1 && GameManager.instance.selectedLevel < LevelManager.instance.levelPrefabs.Length - 1)
        {
            PlayerPrefs.SetInt("Current Level", GameManager.instance.selectedLevel + 1);
        }
        
        StartCoroutine(ShowComplete());
        
        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.ShowInterstitial();
        }
    }
    
    public void NextLevel()
    {
        if (GameManager.instance.selectedLevel < LevelManager.instance.levelPrefabs.Length - 1)
        {
            GameManager.instance.selectedLevel++;
        }
        else
        {
            GameManager.instance.selectedLevel = 1;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ShowComplete()
    {
        yield return new WaitForSeconds(1);

        if (PlayerPrefs.GetInt("All Levels Completed") == 1)
        {
            //Show Congrats Panel
        }
        
        completePanelMoves.text = CubeRollerScript.instance.movesCount.ToString();
        completePanelTargetMoves.text = LevelManager.instance.levelPrefabs[GameManager.instance.selectedLevel].levelTarget.ToString();

        if (CubeRollerScript.instance.movesCount < LevelManager.instance.levelPrefabs[GameManager.instance.selectedLevel].levelCurrentBest || LevelManager.instance.levelPrefabs[GameManager.instance.selectedLevel].levelCurrentBest == 0)
        {
            bestTagText.text = "NEW BEST";
            completePanelBestMoves.text = CubeRollerScript.instance.movesCount.ToString();
            PlayerPrefs.SetInt("Level " + GameManager.instance.selectedLevel + " Best", CubeRollerScript.instance.movesCount);
            LevelManager.instance.levelPrefabs[GameManager.instance.selectedLevel].levelCurrentBest = CubeRollerScript.instance.movesCount;
        }
        else
        {
            bestTagText.text = "BEST";
            completePanelBestMoves.text = LevelManager.instance.levelPrefabs[GameManager.instance.selectedLevel].levelCurrentBest.ToString();
        }
        
        gameplayPanel.gameObject.SetActive(false);
        failPanel.SetActive(false);
        pausePanel.SetActive(false);
        
        completePanel.SetActive(true);
        
        /*if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.ShowSmallBanner();
        }*/
    }
    
    bool once = false;
    public void FailLevel()
    {
        CubeRollerScript.instance.controlsActive = false;
        
        StartCoroutine(ShowFail());
        
        if (!once)
        {
            once = true;
            if (GoogleMobileAdsManager.Instance != null)
            {
                GoogleMobileAdsManager.Instance.ShowInterstitial();
            }
        }
    }
    IEnumerator ShowFail()
    {
        yield return new WaitForSeconds(1);
        
        gameplayPanel.gameObject.SetActive(false);
        pausePanel.SetActive(false);
        completePanel.SetActive(false);
        
        failPanel.SetActive(true);

        if (SoundManager.instance != null)
        {
            SoundManager.instance.lose.Play();
        }
        
        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.ShowSmallBanner();
        }
    }

    

    public void GotoHome()
    {
        loadingPanel.SetActive(true);

        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.ShowSmallBanner();
        }

        StartCoroutine(ShowInterstitialAd());
        StartCoroutine(LoadMenuScene());
    }

    IEnumerator LoadMenuScene()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("Menu");
    }
    
    IEnumerator ShowInterstitialAd()
    {
        yield return new WaitForSecondsRealtime(1);
        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.ShowInterstitial();
        }
    }
}
