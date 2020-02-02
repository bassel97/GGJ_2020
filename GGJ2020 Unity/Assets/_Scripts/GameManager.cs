using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //TODO Also rewrite this class

    private bool startedPlaying = false;

    public GameObject playerObject;
    public Text instrText;

    public GameObject[] camsObject;

    public AudioSource gameTrack;
    public AudioSource camTrack;

    private void Update()
    {
        if (!startedPlaying)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TimeManager.instance.ResetManager();

                playerObject.SetActive(true);

                instrText.gameObject.SetActive(false);

                GetComponent<PeopleManager>().ResetScene();

                for (int i = 0; i < camsObject.Length; i++)
                {
                    camsObject[i].SetActive(false);
                }

                startedPlaying = true;

                camTrack.Stop();
                gameTrack.Play();
            }
        }

        //Quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //RestartScene
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
