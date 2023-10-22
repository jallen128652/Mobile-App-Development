using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    //link to our background music audio source
    public AudioSource myAudio;

    //custom method to change audio volume based on slider movement
    public void OnChangeVolume()
    {
        //set the myAudio to match the slider
        myAudio.volume = GetComponent<Slider>().value;
    }


    // Start is called before the first frame update
    void Start()
    {
        //Link to our slider component
        GetComponent<Slider>().value = .5f;

        //set the myAudio to match the slider
        myAudio.volume = GetComponent<Slider>().value;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
