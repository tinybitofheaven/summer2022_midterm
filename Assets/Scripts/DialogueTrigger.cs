using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class DialogueTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip textboxClip;
    public AudioClip warpClip;

    public int minigame; //scene number
    private bool _canTrigger = false;

    public Sprite ghostImg;
    // public Sprite monsterImg;
    public GameObject textImg;
    public GameObject textbox;
    public GameObject spaceTxt;
    public string[] text;
    private int index = 0;

    private void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        if (text.Length == 0)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            textImg.SetActive(true);
            textbox.SetActive(true);
            spaceTxt.SetActive(true);
            textbox.GetComponent<TextMeshProUGUI>().text = text[index];
            _canTrigger = true;
        }
    }

    private void Update()
    {
        if (_canTrigger == true && Input.GetKeyDown(KeyCode.Space))
        {
            index++;
            if (index < text.Length)
            {
                audioSource.clip = textboxClip;
                audioSource.Play();

                if (text[index] == "-1")
                {
                    index = 0;
                    textImg.SetActive(false);
                    textbox.SetActive(false);
                    spaceTxt.SetActive(false);
                    _canTrigger = false;
                    Destroy(GameObject.FindGameObjectWithTag("Barrier"));
                    Destroy(this);
                }
                else
                {
                    textbox.GetComponent<TextMeshProUGUI>().text = text[index];
                }
            }
            else
            {
                SceneChange();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            index = 0;
            textImg.SetActive(false);
            textbox.SetActive(false);
            spaceTxt.SetActive(false);
            _canTrigger = false;

            if (gameObject.name == "empty")
            {
                //stop music
                GameManager.FindInstance().GetComponent<AudioSource>().Pause();
            }
        }
    }

    private void SceneChange()
    {
        if (minigame != 0)
        {
            if (gameObject.name == "monster")
            {
                //boss music
                GameManager.FindInstance().GetComponent<AudioSource>().clip = GameManager.FindInstance().bossMusic;
                // GameManager.FindInstance().GetComponent<AudioSource>().Play();
            }
            else
            {
                audioSource.clip = warpClip;
                audioSource.Play();
            }

            index = 0;
            textImg.SetActive(false);
            textbox.SetActive(false);
            spaceTxt.SetActive(false);
            _canTrigger = false;

            GameManager _gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            _gm.playerX = gameObject.transform.position.x;
            _gm.playerY = gameObject.transform.position.y;
            _gm.playerZ = gameObject.transform.position.z;
            SceneManager.LoadScene(minigame);
        }
    }
}
