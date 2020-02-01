using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facility
{
    WaterSource,
    Chair,
    ExitLoc
}

public class FacilitiesManager : MonoBehaviour
{
    //private bool isSetUp = false;
    private GameObject[] waterSources;
    private GameObject[] chairs;
    private GameObject[] exitLocations;

    public static FacilitiesManager instance;


    private void Awake()
    {
        instance = this;
        SetUpManager();
    }

    private void SetUpManager()
    {
        waterSources = GameObject.FindGameObjectsWithTag("WaterSource");
        chairs = GameObject.FindGameObjectsWithTag("Chair");
        exitLocations = GameObject.FindGameObjectsWithTag("ExitLocation");

        //isSetUp = true;
    }

    public Transform ReturnNearest(Facility facility, Vector3 pos)
    {
        float minDistance = float.MaxValue;
        Transform nearestLocation = null;

        GameObject[] objectsArray = null;

        switch (facility)
        {
            case Facility.WaterSource:
                objectsArray = waterSources;
                break;

            case Facility.Chair:
                objectsArray = chairs;
                break;

            case Facility.ExitLoc:
                objectsArray = exitLocations;
                break;
            default:
                break;
        }

        for (int i = 0; i < objectsArray.Length; i++)
        {
            float sqrDist = Vector3.SqrMagnitude(pos - objectsArray[i].transform.position);
            if (sqrDist < minDistance)
            {
                minDistance = sqrDist;
                nearestLocation = objectsArray[i].transform;
            }
        }


        return nearestLocation;
    }
}
