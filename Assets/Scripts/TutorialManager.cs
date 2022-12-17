using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] instructions;
    private int inst_index = 0;

    public GameObject instantiation;
    public GameObject player;
    public GameObject objective;
    public GameObject claw;
    public GameObject clawBottom;

    public TMP_Text countdown;
    public GameObject skip;

    void Update()
    {
        //makes certain steps visible
        for (int i = 0; i < instructions.Length; i++)
        {
            if (i == inst_index)
            {
                instructions[i].SetActive(true);
            }
            else
            {
                instructions[i].SetActive(false);
            }
        }

        //step 1: move the player
        if (inst_index == 0)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("NextStep"))
            {
                inst_index++;
                instantiation.SetActive(true);
            }
        }
        //step 2: collect stuffy
        else if (inst_index == 1)
        {
            if (player.GetComponent<PlayerTutorial>().rescuedStuffies.Count == objective.GetComponent<GameObjective>().totalAnimals || Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("NextStep"))
            {
                inst_index++;
            }
        }
        //step 3: throw stuffy
        else if (inst_index == 2)
        {
            if (player.GetComponent<PlayerTutorial>().rescuedStuffies.Count == 0 || Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("NextStep"))
            {
                inst_index++;
            }
        }
        //step 4: tell player their goal is to collect as many stuffies as they can
        else if (inst_index == 3)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("NextStep"))
            {
                inst_index++;
                instantiation.SetActive(false);
                claw.GetComponent<ClawMovementTutorial>().enabled = false;
                claw.transform.position = new Vector2(0, 0);
                clawBottom.transform.position = new Vector2(0, -0.6f);
                player.transform.position = new Vector2(-0.673f, -3.94f);
                player.GetComponent<PlayerTutorial>().enabled = false;

                GameObject[] plushies = GameObject.FindGameObjectsWithTag("Animal");
                GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
                

                foreach (GameObject plush in plushies)
                    GameObject.Destroy(plush);

                foreach (GameObject obstacle in obstacles)
                    GameObject.Destroy(obstacle);

                    
            }
        }
        //step 5: ready to play?
        else if (inst_index == 4)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("NextStep"))
            {
                inst_index++;
                skip.SetActive(false);
                StartCoroutine(countDown(3));
            }
        }

        //skip the tutorial at any point of scene
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("SkipTutorial"))
        {
            inst_index = 5;
            skip.SetActive(false);
            instantiation.SetActive(false);
            claw.GetComponent<ClawMovementTutorial>().enabled = false;
            claw.transform.position = new Vector2(0, 0);
            clawBottom.transform.position = new Vector2(0, -0.6f);
            player.transform.position = new Vector2(-0.673f, -3.94f);
            player.GetComponent<PlayerTutorial>().enabled = false;
            GameObject[] plushies = GameObject.FindGameObjectsWithTag("Animal");
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

            foreach (GameObject plush in plushies)
                GameObject.Destroy(plush);

            foreach (GameObject obstacle in obstacles)
                GameObject.Destroy(obstacle);

            StartCoroutine(countDown(3));
        }
    }

    //countdown to go to the mainscene
    IEnumerator countDown(int seconds)
    {
        int count = seconds;

        while (count > 0)
        {
            countdown.SetText("" + count);
            yield return new WaitForSeconds(1);
            count--;
        }
        SceneManager.LoadScene("MainGame");
    }
}
