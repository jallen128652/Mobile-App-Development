using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//link to unity video player
using UnityEngine.Video;

public class CameraMove : MonoBehaviour
{
    //speed to move camera
    public float speedFactor = 0.1f;

    //anchor to move to
    public Transform anchorToMoveTo;

    //link to transition audio source
    public AudioSource slide;

    //link to background audiosource on the main menu
    public AudioSource background;

    //link to the video screen to clean up when returning to the main screen
    public GameObject VideoScreen;

    //method to pause background musinc from the main menu when going to the video screen
    public void MusicPause()
    {
        //pause audio source playing the background music
        background.Pause();
    }

    //method to play background music and clean up video when returning to the main menu from video screen
    public void MusicStart()
    {
        //play audio source playing the background music
        background.Play();

        //pause video
        VideoScreen.GetComponent<VideoControl>().shouldPlay = false;

        //rewind video
        VideoScreen.GetComponent<VideoControl>().RewindVideo();
    }

    //custom method to take in the new anchor to move to from button press
    public void SetAnchor(Transform anchor)
    {
        //play transition audio
        slide.Play();

        //set new anchor point to move to
        anchorToMoveTo = anchor;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //move camera
        transform.position = Vector3.Lerp(transform.position, anchorToMoveTo.position, speedFactor);

        //rotate camera not in use in this app
        //transform.rotation = Quaternion.Slerp(transform.rotation, anchorToMoveTo.rotation, speedFactor);
    }

}
