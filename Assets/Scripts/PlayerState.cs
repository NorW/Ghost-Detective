using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private static PlayerState instance = null;

    private HashSet< string > playerTags = new HashSet<string>();

    public HashSet< string > PlayerTags
    {
        get { return playerTags; }
    }

    public static PlayerState Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if ( instance == null )
        {
            instance = this;
        }
    }
    public void AddTags( string[] tags )
    {
        foreach( string tag in tags )
        {
            playerTags.Add( tag );
        }
    }

    public void RemoveTags( string[] tags )
    {
        if ( tags == null )
        {
            return;
        }

        foreach ( string tag in tags )
        {
            playerTags.Remove( tag );
        }
    }

    public bool HasAllTags( string[] tags )
    {
        if( tags == null)
        {
            return true;
        }

        foreach( string tag in tags )
        {
            if( !playerTags.Contains( tag ) )
            {
                return false;
            }
        }

        return true;
    }

    public bool HasAnyTag( string[] tags )
    {
        if ( tags == null )
        {
            return false;
        }

        foreach ( string tag in tags )
        {
            if ( playerTags.Contains( tag ) )
            {
                return true;
            }
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
