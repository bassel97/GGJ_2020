using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    [Header("Population Data")]
    [SerializeField] private int noOfPeople = 0;
    [SerializeField] [Range(0, 0.5f)] private float robbersPerc = 0;

    [Header("Prefabs")]
    [SerializeField] private GameObject humanPrefab = null;

    private List<Human> people = new List<Human>();


    private void Start()
    {
        PopulateArea();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < people.Count; i++)
            {
                people[i].ResetHuman();
            }
        }
    }

    public void PopulateArea()
    {
        people.Clear();

        for (int i = 0; i < noOfPeople; i++)
        {
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            GameObject humanGO = Instantiate(humanPrefab, pos, rot);
            Human human = humanGO.GetComponent<Human>();
            human.SetStartParams(pos, rot);

            if (Random.Range(0, 1.0f) > robbersPerc)
            {

            }
            
            people.Add(human);
            human.SetNoOfActions(5);
            human.SetUpActions();
            human.StartNextAction();
        }
    }
}
