using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    [Header("Population Data")]
    [SerializeField] private int noOfPeople = 0;
    [SerializeField] [Range(0, 0.5f)] private float attackersPerc = 0;
    [SerializeField] private Vector2 xBorder = Vector2.zero;
    [SerializeField] private Vector2 zBorder = Vector2.zero;

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
            ResetScene();
        }
    }

    public void ResetScene()
    {
        FacilitiesManager.instance.ResetFacilities();
        for (int i = 0; i < people.Count; i++)
        {
            people[i].ResetHuman();
        }
        TimeManager.instance.ResetManager();
    }

    public void PopulateArea()
    {
        people.Clear();

        for (int i = 0; i < noOfPeople; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(xBorder.x, xBorder.y), 0, Random.Range(zBorder.x, zBorder.y));
            Quaternion rot = Quaternion.Euler(Random.insideUnitCircle.normalized);

            GameObject humanGO = Instantiate(humanPrefab, pos, rot);
            humanGO.transform.parent = transform;
            Human human = humanGO.GetComponent<Human>();
            human.SetStartParams(pos, rot);
            
            people.Add(human);
            human.SetNoOfActions(12);
            human.SetUpActions();

            if (Random.Range(0, 1.0f) > attackersPerc)
            {
                human.SetIsAttacker();
            }

            human.StartNextAction();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color drawColor = Color.yellow;
        drawColor.a = 0.5f;
        Gizmos.color = drawColor;
        Gizmos.DrawCube(transform.position, new Vector3(xBorder.x - xBorder.y, .5f, zBorder.x - zBorder.y));
    }

}
