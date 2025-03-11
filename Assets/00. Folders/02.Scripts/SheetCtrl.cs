using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sheet
{
    [Tooltip("Shooter to Check - 2 sec / Check to Check - 1 sec")]
    public float timing;
    public int type;
    public float var;
}

public class SheetCtrl : MonoBehaviour
{
    public Sheet[] Sheet;
}
