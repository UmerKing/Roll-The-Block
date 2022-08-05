using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject exitPanel;
    void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!exitPanel.activeSelf)
            {
                exitPanel.SetActive(true);
            }
            else
            {
                exitPanel.SetActive(false);
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
