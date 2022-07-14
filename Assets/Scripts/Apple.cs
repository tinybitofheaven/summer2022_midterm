using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Apple : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip textboxClip;
    public AudioClip warpClip;

    public int exitScene = 4; //scene number
    private bool _canTrigger = false;

    public Sprite ghostImg;
    // public Sprite monsterImg;
    public GameObject textImg;
    public GameObject textbox;
    public GameObject spaceTxt;
    public string[] text;
    private int index = 0;

    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
    }

    private void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        text = new string[] {
        "Yes! We managed to beat Anxiety. Wait, they dropped something after they ran away.",
        "It must be the lost artifact! Let's go take a look.",
        "Wait...",
        "Huh? It's just an apple.",
        "Where could the artifact have gone, there's nowhere else the thief could have went.",
        "Would you like to pick up the apple? (Space to pick up)", "-1"};
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
        if (GameManager.FindInstance().beatAnxiety)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (_canTrigger == true && Input.GetKeyDown(KeyCode.Space))
        {
            index++;
            if (index < text.Length)
            {
                audioSource.clip = textboxClip;
                audioSource.Play();

                if (text[index] == "-1")
                {
                    // index = 0;
                    // textImg.SetActive(false);
                    // textbox.SetActive(false);
                    // spaceTxt.SetActive(false);
                    textbox.GetComponent<TextMeshProUGUI>().text = "Woah!";
                    _canTrigger = false;
                    Destroy(gameObject);
                    SceneChange();
                }
                else
                {
                    textbox.GetComponent<TextMeshProUGUI>().text = text[index];
                }
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
        if (exitScene != 0)
        {

            audioSource.clip = warpClip;
            audioSource.Play();

            // index = 0;
            _canTrigger = false;
            SceneManager.LoadScene(exitScene);
        }
    }
}
