using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TileController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip correctClip;
    public AudioClip flipClip;

    public List<Button> btns = new List<Button>();
    public Transform puzzleField;
    public GameObject btn;
    public Sprite bgImg;
    public Sprite[] imgs;
    public List<Sprite> gameImgs = new List<Sprite>();
    public bool firstGuess, secondGuess;
    private int moves = 0;
    private int clearedPairs;
    private int matchPairs;
    public string firstGuessImg, secondGuessImg;
    public int firstGuessIndex, secondGuessIndex;
    public GameObject scoreTxt;
    public ParticleSystem explosion1;
    public ParticleSystem explosion2;


    private void Awake()
    {
        //create buttons
        for (int i = 0; i < 20; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;
            button.transform.SetParent(puzzleField, false);
        }
    }

    private void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        GetButtons();
        AddListeners();
        AddGameTiles();
        Shuffle(gameImgs);
        matchPairs = gameImgs.Count / 2;
    }

    private void Update()
    {
        scoreTxt.GetComponent<TextMeshProUGUI>().text = "Moves Used: " + moves;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImg;
        }
    }

    private void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.GetComponent<Button>().onClick.AddListener(() => ButtonClick());
        }
    }

    private void AddGameTiles()
    {
        int looper = btns.Count;
        int index = 0;
        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }
            gameImgs.Add(imgs[index]);
            index++;
        }
    }

    public void ButtonClick()
    {
        string btnName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if (!firstGuess)
        {
            audioSource.clip = flipClip;
            audioSource.Play();

            firstGuess = true;
            firstGuessIndex = int.Parse(btnName);
            firstGuessImg = gameImgs[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gameImgs[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            if (firstGuessIndex != int.Parse(btnName))
            {
                audioSource.clip = flipClip;
                audioSource.Play();

                secondGuess = true;
                secondGuessIndex = int.Parse(btnName);
                secondGuessImg = gameImgs[secondGuessIndex].name;
                btns[secondGuessIndex].image.sprite = gameImgs[secondGuessIndex];

                moves++;
                StartCoroutine(CheckMatch());
            }
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f);
        if (firstGuessImg == secondGuessImg)
        {
            yield return new WaitForSeconds(0.5f);
            audioSource.clip = correctClip;
            audioSource.Play();

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = Color.clear;
            btns[secondGuessIndex].image.color = Color.clear;

            this.explosion1.transform.position = btns[firstGuessIndex].transform.position;
            this.explosion2.transform.position = btns[secondGuessIndex].transform.position;
            this.explosion1.Play();
            this.explosion2.Play();

            CheckFinished();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            btns[firstGuessIndex].image.sprite = bgImg;
            btns[secondGuessIndex].image.sprite = bgImg;
        }
        yield return new WaitForSeconds(0.5f);
        firstGuess = secondGuess = false;
    }

    private void CheckFinished()
    {
        clearedPairs++;
        if (clearedPairs == matchPairs)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
