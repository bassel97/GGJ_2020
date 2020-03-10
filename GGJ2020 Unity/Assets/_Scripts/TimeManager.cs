using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Text[] camsTime = null;

    public static TimeManager instance;
    private void Awake()
    {
        instance = this;
    }

    public float initialTime;
    public float timeValue
    {
        get;
        private set;
    }

    void Start()
    {
        initialTime = Random.Range(0, 10000);
        timeValue = initialTime;
    }

    private void Update()
    {
        timeValue += Time.deltaTime;

        for (int i = 0; i < camsTime.Length; i++)
        {
            camsTime[i].text = GetTimeString();
        }
    }

    public void ResetManager()
    {
        timeValue = initialTime;
    }

    public string GetTimeString()
    {
        string timeString = "";

        timeString += (int)((timeValue) / 60) % 60;
        timeString += ":";
        timeString += (int)(timeValue) % 60;
        timeString += "\n";

        return timeString;
    }
}
