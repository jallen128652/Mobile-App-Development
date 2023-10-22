using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//imports for ui
using UnityEngine.UI;

public class TakePhoto : MonoBehaviour
{
    //link to the raw image in the hierarchy
    public RawImage rawImage;

    //create a webcamtexture var which is internal and not accessible by the
    //inspector
    WebCamTexture cameraTex;

    //int  accumulator
    public int captureCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //set up webcam texture obj reference
        WebCamTexture webCamTexture = new WebCamTexture();

        //set it's width and height
        webCamTexture.requestedWidth = 1300;
        webCamTexture.requestedHeight = 900;

        //link to the rawImage texture that is drawing the camer image onto the screen
        rawImage.texture = webCamTexture;
        rawImage.material.mainTexture = webCamTexture;

        //play the webcam texture - update it on scree
        webCamTexture.Play();

        //link our cameraTex we will be pulling images from to the webcam texure showing on screen
        cameraTex = webCamTexture;

        //check to see if our player pref exists - counter
        if (PlayerPrefs.HasKey("counter"))
        {
            //if so load into captureCounter
            captureCounter = PlayerPrefs.GetInt("counter");
        }
        else
        {
            // if it doesn't exist, create it
            PlayerPrefs.SetInt("counter", 0);

            //save to disk
            PlayerPrefs.Save();

            //load into captureCounter
            captureCounter = PlayerPrefs.GetInt("counter");
        }
    }

    //Our snapshot code
    public void TakeSnapShot()
    {
        //sets up a texture 2d the same size as the webcamtexture cameratex
        Texture2D snap = new Texture2D(cameraTex.width, cameraTex.height);

        //get all the pixels from the cameratex and place them into the snap texture2d
        // getters and setters
        snap.SetPixels(cameraTex.GetPixels());

        //apply/draw the pixels into the texture2d
        snap.Apply();

        //platform check to see if we are in the editor
#if UNITY_EDITOR

        //write the snapshot to the unity project folder
        System.IO.File.WriteAllBytes(Application.dataPath + captureCounter.ToString() + ".png", snap.EncodeToPNG());

#elif UNITY_ANDROID

        //take screenshot and save it to Gallery/Photos
        StartCoroutine( TakeScreenshotAndSave() );

#endif

        //Increase the capture counter int
        ++captureCounter;

        //set the new val
        PlayerPrefs.SetInt("counter", captureCounter);

        //write new val to the disk
        PlayerPrefs.Save();

    }

    //native gallery code
    private IEnumerator TakeScreenshotAndSave()
    {
        //wait for processing
        yield return new WaitForEndOfFrame();

        //sets up a texture2d the same size as the webcamtexture cameratex
        Texture2D snapShot = new Texture2D(cameraTex.width, cameraTex.height, TextureFormat.RGB24, false);

        //read all the pixels from the cameraTex 
        snapShot.ReadPixels(new Rect(328, 200, cameraTex.width, cameraTex.height), 0, 0);

        //apply/draw the pixels into the texture2d
        snapShot.Apply();

        //save the screenshot to Gaallery/Photos
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(snapShot, "GalleryTest", "Image.png"));

        //to avoid memory leaks
        Destroy(snapShot);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
