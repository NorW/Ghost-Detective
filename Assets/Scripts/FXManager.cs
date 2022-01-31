using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class FXManager : MonoBehaviour
{

    public AudioClip [] clips;
    public Dictionary<string, AudioClip> Clips;

    void Awake()
    {
        Clips = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in clips)
        {
            Clips.Add(clip.ToString(), clip);
            Debug.Log(clip.ToString());
        }


    }
    public void playbyname(string clipname)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(Clips[clipname]);

    }
}
