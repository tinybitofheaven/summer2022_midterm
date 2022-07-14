using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 0.5f;
    public Rigidbody2D playerRb;

    private Vector2 _movement;
    // private float _jumpVelocity = 10f;

    //animator
    public Animator playerAnimator;
    private Vector2 _previousPosition;

    // Start is called before the first frame update
    private void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody2D>();
        GameManager _gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameObject.transform.position = new Vector3(_gm.playerX, _gm.playerY, _gm.playerZ);
    }

    // Update is called once per frame
    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");

        if (playerAnimator.GetInteger("direction") == 0) //left
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + _movement * playerSpeed);

        //animation
        if (_movement.x != 0)
        { //isnt moving
            playerAnimator.SetBool("moving", true);
        }
        else
        {
            playerAnimator.SetBool("moving", false);
        }

        if (_movement.x < 0)
        { //left
            playerAnimator.SetInteger("direction", 1);
        }
        if (_movement.x > 0)
        { //right

            playerAnimator.SetInteger("direction", 0);
        }

        _previousPosition = playerRb.position;

    }
}
