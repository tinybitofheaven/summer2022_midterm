using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AsteroidsManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip roarClip;
    public AudioClip screamClip;
    public AudioSource monsterSource;

    //asteriod
    public Arrow arrow;
    public ParticleSystem explosion;
    public int lives = 3;
    public float respawnTime = 3.0f;
    public int health = 1000;

    public GameObject healthTxt;
    public GameObject livesTxt;
    public GameObject wonTxt;

    public SpriteRenderer monster;

    private bool zero, first, second, third = false;

    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        GameManager.FindInstance().GetComponent<AudioSource>().Play();
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        audioSource.clip = hitClip;
        audioSource.Play();

        //health
        if (asteroid.size < 1.4)
        {
            health -= 75;
        }
        else if (asteroid.size < 2f)
        {
            health -= 50;
        }
        else
        {
            health -= 25;
        }
    }


    public void ArrowDied()
    {
        this.explosion.transform.position = this.arrow.transform.position;
        this.explosion.Play();

        audioSource.clip = hitClip;
        audioSource.Play();

        this.lives--;
        if (this.lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    private void Respawn()
    {
        this.arrow.transform.position = Vector3.zero;
        this.arrow.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        this.arrow.gameObject.SetActive(true);

        this.Invoke(nameof(TurnOnCollisions), 3.0f);
    }

    private void TurnOnCollisions()
    {
        this.arrow.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {

        if (healthTxt != null)
            healthTxt.GetComponent<TextMeshProUGUI>().text = "Anxiety Health: " + health;

        if (health <= 0)
        {
            healthTxt.GetComponent<TextMeshProUGUI>().text = "Anxiety Health: " + 0;
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>().spawning = false;
        }

        if (health <= 0 && zero == false)
        {
            monsterSource.clip = screamClip;
            monsterSource.Play();
            zero = true;

            //game won
            wonTxt.SetActive(true);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().beatAnxiety = true;
            StartCoroutine(FlashRed());
        }
        else if (health <= 300 && third == false)
        {
            monsterSource.clip = roarClip;
            monsterSource.Play();
            third = true;
            StartCoroutine(FlashRed());
        }
        else if (health <= 600 && second == false)
        {
            monsterSource.clip = roarClip;
            monsterSource.Play();
            second = true;
            StartCoroutine(FlashRed());
        }
        else if (health <= 900 && first == false)
        {
            monsterSource.clip = roarClip;
            monsterSource.Play();
            first = true;

            StartCoroutine(FlashRed());
        }


        if (livesTxt != null)
            livesTxt.GetComponent<TextMeshProUGUI>().text = "Lives: " + lives;

        if (Input.GetKeyDown(KeyCode.Escape) && arrow != null)
        {
            if (GameManager.FindInstance().beatAnxiety)
            {
                GameManager.FindInstance().GetComponent<AudioSource>().clip = GameManager.FindInstance().bgm;
                GameManager.FindInstance().GetComponent<AudioSource>().Play();
            }
            else
            {
                GameManager.FindInstance().GetComponent<AudioSource>().Pause();
            }
            SceneManager.LoadScene(0);
        }
    }

    public IEnumerator FlashRed()
    {
        monster.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        monster.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        monster.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        monster.color = Color.white;
    }

}
