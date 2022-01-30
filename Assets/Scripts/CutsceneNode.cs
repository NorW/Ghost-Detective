using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneNode : MonoBehaviour
{
    [SerializeField] string nodeName;
    [SerializeField] float size;

    public string NodeName { get { return nodeName; } }
    public float Size { get { return size; } }
}
