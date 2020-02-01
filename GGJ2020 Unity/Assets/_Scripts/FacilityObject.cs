using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityObject : MonoBehaviour
{
    [SerializeField] private bool alwaysFree = false;

    private bool isInUse = false;
    public bool IsInUse()
    {
        return isInUse && !alwaysFree;
    }

    public void SetInUse()
    {
        /*if (name != "Exit")
            Debug.Log(name + " set In Use");*/
        isInUse = true;
    }

    public void SetFree()
    {
        /*if (name != "Exit")
            Debug.Log(name + " set free");*/
        isInUse = false;
    }
}
