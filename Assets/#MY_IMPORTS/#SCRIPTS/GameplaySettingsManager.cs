using UnityEngine;
using UnityEngine.UI;

public class GameplaySettingsManager : MonoBehaviour
{
    [Header("Controls")] 
    public GameObject buttonsController;
    public Toggle buttonsControlToggle, swipeControlToggle;

    [Header("Sound and Music")]
    public Image soundIcon;
    private bool isSoundOn;
    public Toggle soundToggle;
    
    public Image musicIcon;
    private bool isMusicOn;
    public Toggle musicToggle;

    public Sprite soundOn, soundOff, musicOn, musicOff;

    [Header("LINS")]
    public string packageName;
    
    void Start()
    {
        GetSelectedControls();
    }

    void GetSelectedControls()
    {
        //Controls
        if (!PlayerPrefs.HasKey("Is Button"))
        {
            PlayerPrefs.SetInt("Is Button", 1);
            PlayerPrefs.SetInt("Is Swipe", 0);

            buttonsControlToggle.isOn = true;
            swipeControlToggle.isOn = false;
            if (buttonsController != null)
            {
                buttonsController.SetActive(true);
            }

            if (CubeRollerScript.instance != null)
            {
                CubeRollerScript.instance.swipeControls = false;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("Is Swipe") == 1)
            {
                swipeControlToggle.isOn = true;
                buttonsControlToggle.isOn = false;

                if (CubeRollerScript.instance != null)
                {
                    CubeRollerScript.instance.swipeControls = true;
                }

                if (buttonsController != null)
                {
                    buttonsController.SetActive(false);
                }
            }
            else if(PlayerPrefs.GetInt("Is Button") == 1)
            {
                swipeControlToggle.isOn = false;
                buttonsControlToggle.isOn = true;

                if (CubeRollerScript.instance != null)
                {
                    CubeRollerScript.instance.swipeControls = false;
                }

                if (buttonsController != null)
                {
                    buttonsController.SetActive(true);
                }
            }
        }
        
        //Sound and Music

        if (!PlayerPrefs.HasKey("Is Sound On"))
        {
            PlayerPrefs.SetInt("Is Sound On", 1);
            PlayerPrefs.SetInt("Is Music On", 1);
            AudioListener.volume = 1;

            if (SoundManager.instance != null)
            {
                SoundManager.instance.musicAudioSource.volume = SoundManager.instance.maxVolume;
            }

            if (soundIcon != null)
            {
                soundIcon.sprite = soundOn;
            }

            if (musicIcon != null)
            {
                musicIcon.sprite = musicOn;
            }

            isSoundOn = true;
            isMusicOn = true;
            soundToggle.isOn = true;
            musicToggle.isOn = true;
        }
        else
        {
            if (PlayerPrefs.GetInt("Is Sound On") == 1)
            {
                if (soundIcon != null)
                {
                    soundIcon.sprite = soundOn;
                }

                AudioListener.volume = 1;
                isSoundOn = true;
                soundToggle.isOn = true;
            }
            else
            {
                if (soundIcon != null)
                {
                    soundIcon.sprite = soundOff;
                }

                AudioListener.volume = 0;
                isSoundOn = false;
                soundToggle.isOn = false;
            }
            
            if (PlayerPrefs.GetInt("Is Music On") == 1)
            {
                if (musicIcon != null)
                {
                    musicIcon.sprite = musicOn;
                }

                isMusicOn = true;
                musicToggle.isOn = true;
                
                if (SoundManager.instance != null)
                {
                    SoundManager.instance.musicAudioSource.volume = SoundManager.instance.maxVolume;
                }

            }
            else
            {
                if (musicIcon != null)
                {
                    musicIcon.sprite = musicOff;
                }

                isMusicOn = false;
                musicToggle.isOn = false;
                
                if (SoundManager.instance != null)
                {
                    SoundManager.instance.musicAudioSource.volume = 0;
                }

            }
        }
    }

    public void SwipeControl(bool isSwipe)
    {
        if (isSwipe)
        {
            if (CubeRollerScript.instance != null)
            {
                CubeRollerScript.instance.swipeControls = true;
            }

            if (buttonsController != null)
            {
                buttonsController.SetActive(false);
            }

            PlayerPrefs.SetInt("Is Swipe", 1);
            PlayerPrefs.SetInt("Is Button", 0);
        }
    }
    public void ButtonsControl(bool isButtons)
    {
        if (isButtons)
        {
            if (CubeRollerScript.instance != null)
            {
                CubeRollerScript.instance.swipeControls = false;
            }

            if (buttonsController != null)
            {
                buttonsController.SetActive(true);
            }

            PlayerPrefs.SetInt("Is Swipe", 0);
            PlayerPrefs.SetInt("Is Button", 1);
        }
    }

    public void Sound()
    {
        isSoundOn = !isSoundOn;
        if (isSoundOn)
        {
            PlayerPrefs.SetInt("Is Sound On", 1);
            soundIcon.sprite = soundOn;
            AudioListener.volume = 1;
            soundToggle.isOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("Is Sound On", 0);
            soundIcon.sprite = soundOff;
            AudioListener.volume = 0;
            soundToggle.isOn = false;
        }
    }
    public void Music()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn)
        {
            PlayerPrefs.SetInt("Is Music On", 1);
            if (SoundManager.instance != null)
            {
                SoundManager.instance.musicAudioSource.volume = SoundManager.instance.maxVolume;
            }
            musicIcon.sprite = musicOn;
            musicToggle.isOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("Is Music On", 0);
            if (SoundManager.instance != null)
            {
                SoundManager.instance.musicAudioSource.volume = 0;
            }
            musicIcon.sprite = musicOff;
            musicToggle.isOn = false;
        }
    }
    
    //TOGGLES
    public void Sound(bool soundIsOn)
    {
        if (soundIsOn)
        {
            PlayerPrefs.SetInt("Is Sound On", 1);
            if (soundIcon != null)
            {
                soundIcon.sprite = soundOn;
            }

            isSoundOn = true;
            AudioListener.volume = 1;
        }
        else
        {
            PlayerPrefs.SetInt("Is Sound On", 0);
            if (soundIcon != null)
            {
                soundIcon.sprite = soundOff;
            }

            isSoundOn = false;
            AudioListener.volume = 0;
        }
    }
    public void Music(bool musicIsOn)
    {
        if (musicIsOn)
        {
            PlayerPrefs.SetInt("Is Music On", 1);
            if (SoundManager.instance != null)
            {
                SoundManager.instance.musicAudioSource.volume = SoundManager.instance.maxVolume;
            }

            if (musicIcon != null)
            {
                musicIcon.sprite = musicOn;
            }

            isMusicOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("Is Music On", 0);
            if (SoundManager.instance != null)
            {
                SoundManager.instance.musicAudioSource.volume = 0;
            }

            if (musicIcon != null)
            {
                musicIcon.sprite = musicOff;
            }

            isMusicOn = false;
        }
    }

    public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + packageName);
    }
}
