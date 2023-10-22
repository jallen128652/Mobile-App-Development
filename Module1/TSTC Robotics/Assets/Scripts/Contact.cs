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
    string screenShotURL = "http://unityjumpstart.com/phpform/FinalContact2.php";

    //link to the input fields
    public InputField nameIF;
    public InputField addressIF;
    public InputField cityIF;
    public InputField stateIF;
    public InputField zipIF;
    public InputField phoneIF;
    public InputField emailIF;
    public InputField techIF;

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

        //zip
        var zipFix = MyEscapeURL(zipIF.text);
        screenShotURL += "&zip=" + zipFix;

        //phone
        var phoneFix = MyEscapeURL(phoneIF.text);
        screenShotURL += "&phone=" + phoneFix;

        //email
        var emailFix = MyEscapeURL(emailIF.text);
        screenShotURL += "&email=" + emailFix;

        //comments
        var techFix = MyEscapeURL(techIF.text);
        screenShotURL += "&tech=" + techFix;

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
        zipIF.text = "";
        phoneIF.text = "";
        emailIF.text = "";
        techIF.text = "";
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
