using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// for modifying UI elements
using UnityEngine.UI;

public class ImageGallery : MonoBehaviour
{
    //class vars
    //link to button press sound
    public AudioClip button;

    //link to audio source to play sound(camera anchor)
    public AudioSource myAudio;

    //link to the main image in the UI
    public Image imageHolder;

    //link to all the sprite images
    public Sprite[] allSprites;

    //integer tracking current image shown
    public int currentImage = 0;

    //link to the main text in the UI
    public Text textHolder;

    //link to all the descriptive texts
    public string[] alltext;

    //public fx for next image button press
    public void NextRobot()
    {
        //play button press sound
        myAudio.PlayOneShot(button);

        //increase currentimage number
        currentImage++;

        //check to see if we are out of bounds
        if (currentImage > allSprites.Length - 1)
        {
            currentImage = 0;
        }

        //put new sprite into the on screen image component
        imageHolder.sprite = allSprites[currentImage];

        //put new text into the on screen text component
        textHolder.text = alltext[currentImage];
    }

    //public fx for previous image button press
    public void PreviousRobot()
    {
        //play button press sound
        myAudio.PlayOneShot(button);

        //increase currentimage number
        currentImage--;

        //check to see if we are out of bounds
        if (currentImage < 0)
        {
            currentImage = allSprites.Length - 1;
        }

        //put new sprite into the on screen image component
        imageHolder.sprite = allSprites[currentImage];

        //put new text into the on screen text component
        textHolder.text = alltext[currentImage];
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
