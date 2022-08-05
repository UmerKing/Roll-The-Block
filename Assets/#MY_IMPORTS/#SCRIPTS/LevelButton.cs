using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;
    
    private Image thisImage;
    private Button thisButton;
    public Sprite playedSprite, currentSprite, lockedSprite;
    public TMP_Text levelNumberText;
    public GameObject levelTitleText;
    
    public GameObject lockIcon;

    public GameObject stars;
    public GameObject starFill1, starFill2, starFill3;
    
    void Start()
    {
        thisImage = GetComponent<Image>();
        thisButton = GetComponent<Button>();
        levelNumberText.text = levelNumber.ToString();

        if (!PlayerPrefs.HasKey("Level 1"))
        {
            PlayerPrefs.SetInt("Level 1", 1);
            PlayerPrefs.SetInt("Current Level", 1);
        }
        
        GetLevelDetails();
    }

    void GetLevelDetails()
    {
        if (PlayerPrefs.GetInt("Level " + levelNumber) == 1)
        {
            thisButton.interactable = true;
            if (PlayerPrefs.GetInt("Current Level") == levelNumber && PlayerPrefs.GetInt("All Levels Completed") != 1)
            {
                thisImage.sprite = currentSprite;
                
                lockIcon.SetActive(false);
                levelNumberText.gameObject.SetActive(true);
                levelTitleText.SetActive(true);
                stars.SetActive(false);
            }
            else
            {
                thisImage.sprite = playedSprite;
                
                lockIcon.SetActive(false);
                levelNumberText.gameObject.SetActive(true);
                levelTitleText.SetActive(false);
                stars.SetActive(true);
                
                int numberOfStarsGot = PlayerPrefs.GetInt("Level Stars " + levelNumber);

                switch (numberOfStarsGot)
                {
                    case 1:
                    {
                        starFill1.SetActive(true);
                        starFill2.SetActive(false);
                        starFill3.SetActive(false);
                        break;
                    }
                    case 2:
                    {
                        starFill1.SetActive(true);
                        starFill2.SetActive(true);
                        starFill3.SetActive(false);
                        break;
                    }
                    case 3:
                    {
                        starFill1.SetActive(true);
                        starFill2.SetActive(true);
                        starFill3.SetActive(true);
                        break;
                    }
                }
            }
        }
        else
        {
            thisButton.interactable = false;
            lockIcon.SetActive(true);
            levelNumberText.gameObject.SetActive(false);
            levelTitleText.SetActive(false);
            stars.SetActive(false);
        }
    }
}
