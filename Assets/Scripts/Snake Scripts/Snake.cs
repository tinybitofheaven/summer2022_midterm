using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Snake : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip coinClip;
    public AudioClip hitClip;

    public Transform segmentPrefab;
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments;
    private int score = 0;
    public GameObject scoreTxt;
    public int intialSize = 3;

    public GameObject endTxt;
    private bool lost = false;
    public ParticleSystem explosion;

    private void Start()
    {
        Time.fixedDeltaTime = 0.11f;
        audioSource = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();

        _segments = new List<Transform>();
        ResetState();
    }

    private void Update()
    {
        scoreTxt.GetComponent<TextMeshProUGUI>().text = "Score: " + score;

        if (!lost)
        {
            if (Input.GetKeyDown(KeyCode.W) && _direction.y != -1) //make sure can't go backwards
            {
                _direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.A) && _direction.x != 1)
            {
                _direction = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.S) && _direction.y != 1)
            {
                _direction = Vector2.down;
            }
            else if (Input.GetKeyDown(KeyCode.D) && _direction.x != -1)
            {
                _direction = Vector2.right;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameOver();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetState();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneSwitch();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!lost)
        {
            for (int i = _segments.Count - 1; i > 0; i--)
            {
                _segments[i].position = _segments[i - 1].position;
            }

            gameObject.transform.position = new Vector3(
                Mathf.Round(gameObject.transform.position.x) + _direction.x, //make sure is whole value
                Mathf.Round(gameObject.transform.position.y) + _direction.y,
                0f
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        }
        else if (other.tag == "Border")
        {
            GameOver();
        }
    }

    private void Grow()
    {
        audioSource.clip = coinClip;
        audioSource.Play();

        Transform segment = Instantiate(segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
        score += 100;
    }

    private void ResetState()
    {
        endTxt.SetActive(false);
        lost = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        GameObject.FindGameObjectWithTag("Food").GetComponent<Food>().RandomziedPOsition();
        score = 0;

        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(gameObject.transform);

        for (int i = 1; i < intialSize; i++)
        {
            _segments.Add(Instantiate(segmentPrefab));
        }

        gameObject.transform.position = Vector3.zero;
    }

    private void GameOver()
    {
        audioSource.clip = hitClip;
        audioSource.Play();

        lost = true;
        endTxt.SetActive(true);
        _direction = Vector2.zero;
        gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
        this.explosion.transform.position = gameObject.transform.position;
        this.explosion.Play();
    }

    private void SceneSwitch()
    {
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene(0);
    }
}
