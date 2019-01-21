using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{

    public AudioSource[] sources;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayAudio(int source)
    {
        sources[source].Play();
    }

    public void StopAudio(int source)
    {
        sources[source].Stop();
    }

    public void volumeSource(int source, float volume)
    {
        sources[source].volume = volume;
    }



}
