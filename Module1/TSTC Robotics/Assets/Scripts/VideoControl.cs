using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//link to unity video player
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    //link to the video player game obj
    public VideoPlayer videoPlayer;

    //boolean switch controlling if the clip should play
    public bool shouldPlay = false;

    //list of available movie clips
    public VideoClip[] myClips;

    //current clip playing
    public int currentClip = 0;

    //method to move to the next video
    public void NextVideo()
    {
        //increase the clip to play by 1
        currentClip++;

        //call pause video method
        PauseVideo();

        //call rewind video method
        RewindVideo();

        //check to see if the currentClip variable is out of bounds, if so set to 0
        if (currentClip > myClips.Length - 1)
        {
            currentClip = 0;
        }

        //load the chosen video clip
        videoPlayer.clip = myClips[currentClip];
    }

    //method to move to the previous video
    public void PreviousVideo()
    {
        //increase the clip to play by 1
        currentClip--;

        //call pause video method
        PauseVideo();

        //call rewind video method
        RewindVideo();

        //check to see if the currentClip variable is out of bounds, if so set to 0
        if (currentClip < 0)
        {
            currentClip = myClips.Length - 1;
        }

        //load the chosen video clip
        videoPlayer.clip = myClips[currentClip];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if the video should be playing
        if (shouldPlay == true)
        {
            //play the video
            videoPlayer.Play();
        }
        else
        {
            //pause the video
            videoPlayer.Pause();
        }
    }

    //public method used to play the video
    public void PlayVideo()
    {
        //set the bool to play videos to true
        shouldPlay = true;
    }

    //public method used to pause the video
    public void PauseVideo()
    {
        //set the bool to play videos to false
        shouldPlay = false;
    }

    //public method used to rewind the video
    public void RewindVideo()
    {
        //set the frame to zero - rewind
        videoPlayer.frame = 0;

        //set the bool to play videos to false
        shouldPlay = false;
    }
}
