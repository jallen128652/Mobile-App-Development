using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//imports for ui and networking
using UnityEngine.UI;
using UnityEngine.Networking;

public class Contact : MonoBehaviour
{
    //string pointing to localhost server and postData.pjp - for testing php script
    //string screenShotURL = "http://localhost/postData.php";

    //string pointing to online server and postData2.pjp - for testing php script
    string screenShotURL = "https://unityjumpstart.com/phpform/postData2.php";

    //link to the input fields
    public InputField nameIF;
    public InputField addressIF;
    public InputField cityIF;
    public InputField stateIF;
    public InputField commentsIF;

    //method to prepare/format the info to be passed to the php script
    public void SendMail()
    {
        //cleans the input and then appends the data to the string for the php file
        var nameFix = MyEscapeURL(nameIF.text);
        //append nameFix to the string we are going to pass to the php
        screenShotURL += "?name=" + nameFix;

        //same for address
        var addressFix = MyEscapeURL(addressIF.text);
        screenShotURL += "&address=" + addressFix;

        //city
        var cityFix = MyEscapeURL(cityIF.text);
        screenShotURL += "&city=" + cityFix;

        //state
        var stateFix = MyEscapeURL(stateIF.text);
        screenShotURL += "&state=" + stateFix;

        //comments
        var commentsFix = MyEscapeURL(commentsIF.text);
        screenShotURL += "&comments=" + commentsFix;

        //opens the webbrowser and calls the php file with appended data
        Application.OpenURL(screenShotURL);
    }

    //replace the + with an HTML space "%20" for the proper processing on the php side
    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }

    //clears the form and sets all fields to empty
    public void ClearForm()
    {
        nameIF.text = "";
        addressIF.text = "";
        cityIF.text = "";
        stateIF.text = "";
        commentsIF.text = "";
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
