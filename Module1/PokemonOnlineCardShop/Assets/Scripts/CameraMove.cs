using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // camera movement speed
    public float speedFactor = 0.1f;

    // new transform to move to
    public Transform anchorToMoveTo;

    // audio source to play when screen animates
    public AudioSource slide;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move camera
        transform.position = Vector3.Lerp(transform.position, anchorToMoveTo.position, speedFactor);

        //rotate camera if needed
        transform.rotation = Quaternion.Slerp(transform.rotation, anchorToMoveTo.rotation, speedFactor);
    }

    // public method to set the new anchor
    public void SetAnchor(Transform newAnchor)
    {
        //play audio source
        slide.Play();

        //set the anchor to move to
        anchorToMoveTo = newAnchor;
    }
}
