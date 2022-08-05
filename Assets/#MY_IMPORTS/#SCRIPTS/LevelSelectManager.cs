using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public static LevelSelectManager instance;
    public Page[] pages;
    public int currentPage;
    public Button previousButton, nextButton;
    
    [SerializeField] private bool unlockAllLevelCheat;
    public GameObject loadingPanel;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        if (unlockAllLevelCheat)
        {
            UnlockAllLevels();
        }
        
        if (!PlayerPrefs.HasKey("Current Page"))
        {
            PlayerPrefs.SetInt("Current Page", 1);

            currentPage = 1;
            previousButton.interactable = false;
            nextButton.interactable = true;
            if (pages.Length == 2)
            {
                nextButton.interactable = false;
            }
        }
        else
        {
            currentPage = PlayerPrefs.GetInt("Current Page");

            if (pages.Length > 2)
            {

                if (currentPage == 1)
                {
                    previousButton.interactable = false;
                    nextButton.interactable = true;
                }
                else if (currentPage == pages.Length - 1)
                {
                    nextButton.interactable = false;
                    previousButton.interactable = true;
                }
                else
                {
                    previousButton.interactable = true;
                    nextButton.interactable = true;
                }
            }
            else
            {
                previousButton.interactable = false;
                nextButton.interactable = false;
            }
        }
        pages[currentPage].page.SetActive(true);
        pages[currentPage].selectedPagePin.SetActive(true);
    }
    
    void UnlockAllLevels()
    {
        /*for (int i = 1; i <= GameManager.instance.totalLevels; i++)
        {
            PlayerPrefs.SetInt("Level " + i, 1);
        }*/
    }

    public void PreviousPage()
    {
        pages[currentPage].page.SetActive(false);
        pages[currentPage].selectedPagePin.SetActive(false);
        
        if (currentPage > 2)
        {
            currentPage--;
            pages[currentPage].page.SetActive(true);
            pages[currentPage].selectedPagePin.SetActive(true);
            nextButton.interactable = true;
        }
        else
        {
            previousButton.interactable = false;
            nextButton.interactable = true;
            currentPage = 1;
            pages[currentPage].page.SetActive(true);
            pages[currentPage].selectedPagePin.SetActive(true);
        }
    }
    public void NextPage()
    {
        pages[currentPage].page.SetActive(false);
        pages[currentPage].selectedPagePin.SetActive(false);
        if (currentPage < pages.Length - 2)
        {
            previousButton.interactable = true;
            currentPage++;
            pages[currentPage].page.SetActive(true);
            pages[currentPage].selectedPagePin.SetActive(true);
        }
        else
        {
            previousButton.interactable = true;
            nextButton.interactable = false;
            currentPage = pages.Length - 1;
            pages[currentPage].page.SetActive(true);
            pages[currentPage].selectedPagePin.SetActive(true);
        }
    }

    public void OnClickLevel(int levelNumber)
    {
        GameManager.instance.selectedLevel = levelNumber;

        if (currentPage > PlayerPrefs.GetInt("Current Page"))
        {
            PlayerPrefs.SetInt("Current Page", currentPage);
        }

        loadingPanel.SetActive(true);

        StartCoroutine(ShowInterstitialAd());
        StartCoroutine(StartLevel());
    }

    IEnumerator ShowInterstitialAd()
    {
        yield return new WaitForSecondsRealtime(1);
        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.ShowInterstitial();
        }
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Gameplay");
    }
}

[Serializable]
public class Page
{
    public GameObject page;
    public GameObject selectedPagePin;
}
