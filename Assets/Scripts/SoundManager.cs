using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip runLoop;
    public AudioClip jump;
    public AudioClip jumpFall;
    public AudioClip slideLoop;

    private AudioSource audioSource;

    private float timer;
    private string currentLoopName = "";
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void PlayRun()
    {
        // PlayLoop(runLoop);
    }

    private void PlayLoop(AudioClip audioClip)
    {
        timer += Time.deltaTime;
        if (timer > audioClip.length || !currentLoopName.Equals(audioClip.name))
        {
            audioSource.PlayOneShot(audioClip);
            currentLoopName = audioClip.name;
            timer = 0;
        }
    }

    public void PlayJump()
    {
        Stop();
        audioSource.PlayOneShot(jump);
    }

    public void PlayFall()
    {
        Stop();
        audioSource.PlayOneShot(jumpFall);
    }
}
