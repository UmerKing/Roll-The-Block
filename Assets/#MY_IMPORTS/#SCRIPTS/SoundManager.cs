using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource musicAudioSource;
    [Range(0, 1)]
    public float maxVolume;

    public AudioSource starPickup, move, death, finish, finish2, win, lose, swipeOrMoveClick;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
