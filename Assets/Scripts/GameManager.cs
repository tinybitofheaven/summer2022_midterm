using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    //music: https://www.chosic.com/download-audio/32020/
    //font: https://managore.itch.io/m5x7
    private static GameManager instance;
    public Vector3 playerPosition;
    public float playerX = -7f;
    public float playerY = -3.5f;
    public float playerZ = 0f;

    public bool beatAnxiety = false;

    public AudioClip bossMusic;
    public AudioClip bgm;


    public static GameManager FindInstance()
    {
        return instance; //that's just a singletone as the region says
    }

    void Awake() //this happens before the game even starts and it's a part of the singletone
    {
        if (instance != null && instance != this)
        {
            Debug.Log("destroy");
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            Debug.Log("destroy");
            DontDestroyOnLoad(this);
            instance = this;
        }
    }

    void Start()
    {
        playerPosition = new Vector3(playerX, playerY, playerZ);

        GameObject[] triggers = GameObject.FindGameObjectsWithTag("Dialogue");
        foreach (GameObject trigger in triggers)
        {
            trigger.GetComponent<Renderer>().enabled = false;
        }

        GameObject[] borders = GameObject.FindGameObjectsWithTag("Border");
        foreach (GameObject border in borders)
        {
            border.GetComponent<Renderer>().enabled = false;
        }

    }

    private void Update()
    {
        if (beatAnxiety)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject obj in objs)
            {
                Destroy(obj);
            }
        }
    }
}
