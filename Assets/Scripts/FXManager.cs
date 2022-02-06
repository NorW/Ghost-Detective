using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class FXManager : MonoBehaviour
{
    private static FXManager instance = null;
    public static FXManager Instance { get { return instance; } }

    public AudioClip [] clips;
    public Dictionary<string, AudioClip> Clips;

    void Awake()
    {
        if ( instance == null )
        {
            instance = this;
            Clips = new Dictionary<string, AudioClip>();

            foreach ( AudioClip clip in clips )
            {
                Clips.Add( clip.name, clip );
                Debug.Log( clip.name );
            }
        }

    }
    public void playbyname(string clipname)
    {
        if ( Clips.ContainsKey( clipname ) )
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot( Clips[ clipname ] );
            Debug.Log( "Playing SFX: " + clipname );
        }
        else
        {
            Debug.Log( "Unable to find SFX: " + clipname );
        }
    }
}
