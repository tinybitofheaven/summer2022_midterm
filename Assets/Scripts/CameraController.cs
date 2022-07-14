using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f; //lower snaps faster to player
    public Vector3 offset;

    private Vector3 _velocity = Vector3.zero;
    private PlayerController _player;

    //boundaries
    public float limitLeft, limitRight, limitBottom, limitTop;

    // Start is called before the first frame update
    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            _player = target.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Transform currentTarget = target;
        Vector3 desiredPosition = currentTarget.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed); //instead of lerp, good for things that are moving
        //transform.position = smoothedPosition;

        //boundaries
        transform.position = new Vector3(Mathf.Clamp(smoothedPosition.x, limitLeft, limitRight), Mathf.Clamp(smoothedPosition.y, limitBottom, limitTop), smoothedPosition.z);
    }
}
