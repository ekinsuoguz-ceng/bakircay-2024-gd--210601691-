using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource Music;
    public AudioSource AudioSourceDragStatus;
    
    public AudioClip FirstMatch;
    public AudioClip RightMatch;
    public AudioClip WrongMatch;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Eğer sahneler arasında taşınmasını istiyorsanız
        }
        else
        {
            Destroy(gameObject); // Zaten bir instance varsa, yenisini yok et
        }
    }
    
    public void PlayFirstMatch()
    {
        AudioSourceDragStatus.clip = FirstMatch;
        AudioSourceDragStatus.Play();
    }
    public void PlayRightMatch()
    {
        AudioSourceDragStatus.clip = RightMatch;
        AudioSourceDragStatus.Play();
    }
    public void PlayWrongMatch()
    {
        AudioSourceDragStatus.clip = WrongMatch;
        AudioSourceDragStatus.Play();
    }
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnMuteToggleChanged()
    {
        // Tüm sesleri aç/kapat
        AudioListener.pause = !AudioListener.pause ;
    }


    public void OnMuteMusicToggleChanged()
    {
        Music.mute = !Music.mute;
    }

}
