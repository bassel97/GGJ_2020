using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facility
{
    WaterSource,
    Chair,
    ExitLoc,
    Cashier
}

public class FacilitiesManager : MonoBehaviour
{
    //private bool isSetUp = false;
    private GameObject[] waterSources;
    private GameObject[] chairs;
    private GameObject[] exitLocations;
    private GameObject[] cashiers;

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
        cashiers = GameObject.FindGameObjectsWithTag("Cashier");

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

            case Facility.Cashier:
                objectsArray = cashiers;
                break;

            default:
                break;
        }

        for (int i = 0; i < objectsArray.Length; i++)
        {
            //TODO clean for performance
            FacilityObject facilityObject = objectsArray[i].GetComponent<FacilityObject>();

            if (facilityObject.IsInUse())
                continue;

            float sqrDist = Vector3.SqrMagnitude(pos - objectsArray[i].transform.position);
            if (sqrDist < minDistance)
            {
                minDistance = sqrDist;
                nearestLocation = objectsArray[i].transform;
            }
        }

        return nearestLocation;
    }

    public Transform ReturnNearestCashier(Vector3 pos)
    {
        float minDistance = float.MaxValue;
        Transform nearestLocation = null;

        GameObject[] objectsArray = cashiers;

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

    public void ResetFacilities()
    {
        for (int i = 0; i < waterSources.Length; i++)
        {
            waterSources[i].GetComponent<FacilityObject>().SetFree();
        }
        for (int i = 0; i < chairs.Length; i++)
        {
            chairs[i].GetComponent<FacilityObject>().SetFree();
        }
        for (int i = 0; i < exitLocations.Length; i++)
        {
            exitLocations[i].GetComponent<FacilityObject>().SetFree();
        }
        for (int i = 0; i < cashiers.Length; i++)
        {
            cashiers[i].GetComponent<FacilityObject>().SetFree();
        }
    }
}
