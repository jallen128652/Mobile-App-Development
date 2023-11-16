using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//Firebase libraries
using Firebase;
using Firebase.Storage;
using Firebase.Database;
using Firebase.Extensions;
//C# System libraries
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;

public class CardManager : MonoBehaviour
{
    // has firebase been initialized
    public bool isFirebaseInitialized = false;

    //var to track number of cards
    public int numberOfCards;

    //var to track current card
    public int currentCard;

    //var to hold all card names in a list
    public List<string> cardNames = new List<string>();

    //**************VI for view, UI for update, BI for delete*****************

    //var to link the text ui elements
    public Text VIname, VIcentering, VIcorners, VIedges, VIsurface, VIfinalGrade, VIstock;

    //vars to link the text ui elements and input fields
    public InputField UIcentering, UIcorners, UIedges, UIsurface, UIfinalGrade, UIstock;

    public Text UIname;

    //var to link the text ui elements
    public Text BIname, BIcentering, BIcorners, BIedges, BIsurface, BIfinalGrade, BIstock;

    public Button buyCard;

    // firebase database vars

    //get the root reference location of the database
    DatabaseReference baseRef;

    //get the child obj
    DatabaseReference cardsRef;

    //var containing the database snapshot available to all in the code base
    public DataSnapshot publicData;

    // firebase storage vars

    //get a reference to the storage service using default firebase app
    FirebaseStorage storage;

    //create a storage reference from our storage service
    StorageReference storage_ref;

    // create a storage reference to Cards
    StorageReference images_ref;

    //string for URL to image
    public string imageURL;

    // link to the image that shows the cards image
    public RawImage VIcardsImage;

    // link to the image that shows the cards image
    public RawImage UIcardsImage;

    // link to the image that shows the cards image
    public RawImage BIcardsImage;

    //loading image
    public Texture loadingImage;

    //bools to force redraw
    public bool redrawCardImage;
    public bool firstLoad = false;

    // Start is called before the first frame update
    void Start()
    {
        //get the root reference location of the db
        baseRef = FirebaseDatabase.DefaultInstance.RootReference;

        //Get the child obj 
        cardsRef = baseRef.Child("Cards");

        //create link to firebase storage
        storage = FirebaseStorage.DefaultInstance;

        //create ref to google firebase dir
        storage_ref = storage.GetReferenceFromUrl("gs://pokemon-online-card-shop.appspot.com");

        //link to specific dir
        images_ref = storage_ref.Child("Pokemon/Cards");

        // when app starts, check to make sure we have the required dependencies
        // to use firebase, and if not, add them if possible
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;

            if(dependencyStatus == Firebase.DependencyStatus.Available)
            {
                //create and hold a ref to your firebase app, where app is a firebase.
                //FirebaseApp property of your app class
                FirebaseApp app = FirebaseApp.DefaultInstance;

                //initialize firebase
                InitializeFirebase();
            }
            else
            {
                //firebase unity sdk is not safe to use here.
                UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));

                isFirebaseInitialized = false;
            }
        });
    }

    //init the firebase database
    protected virtual void InitializeFirebase()
    {
        //start the listener that listens for the changes in the data of the db
        StartListener();

        // everything is ready
        isFirebaseInitialized = true;
    }

    //this method will listen for any changes to the real-time db
    protected void StartListener()
    {
       // constantly checking for changes in the data in the db - specifically cards category
       FirebaseDatabase.DefaultInstance.GetReference("Cards").ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
       {
           //take the data from the arg "e2" that updates when the data changes in the db
           // and link it to a snapshot available to the rest of the code base
           publicData = e2.Snapshot;

           //if error, report
           if (e2.DatabaseError != null)
           {
               Debug.LogError(e2.DatabaseError.Message);
               return;
           }

           // set total number of cards
           if (numberOfCards == 0) 
           {
               numberOfCards = (int)e2.Snapshot.ChildrenCount;
           }

           //cycle through the snapshot data
           foreach (DataSnapshot cards in e2.Snapshot.Children) 
           {
               //set up list to hold the card names
               if (cardNames.Count < numberOfCards)
               {
                   cardNames.Add(cards.Key.ToString());
               }

               //check to see if the card in the datasnapshot is the same as the current card in our list
               if (cards.Key.ToString().Equals(cardNames[currentCard])) 
               {
                   //******View Screen******
                   //if so, write the shirt name to the View screen in the shirt title field
                   VIname.text = cards.Key.ToString();

                   //set the text for each categor in the View scene view
                   VIcentering.text = "Centering: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Centering").Value.ToString();
                   VIcorners.text = "Corners: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Corners").Value.ToString();
                   VIedges.text = "Edges: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Edges").Value.ToString();
                   VIsurface.text = "Surface: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Surface").Value.ToString();
                   VIfinalGrade.text = "Final Grade: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Final Grade").Value.ToString();
                   VIstock.text = "Stock: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Stock").Value.ToString();

                   //******Update Screen******
                   //if so, write the shirt name to the Update screen in the shirt title field
                   UIname.text = cards.Key.ToString();

                   //set the text for each categor in the Update scene view
                   UIcentering.text = e2.Snapshot.Child(cardNames[currentCard]).Child("Centering").Value.ToString();
                   UIcorners.text = e2.Snapshot.Child(cardNames[currentCard]).Child("Corners").Value.ToString();
                   UIedges.text = e2.Snapshot.Child(cardNames[currentCard]).Child("Edges").Value.ToString();
                   UIsurface.text = e2.Snapshot.Child(cardNames[currentCard]).Child("Surface").Value.ToString();
                   UIfinalGrade.text = e2.Snapshot.Child(cardNames[currentCard]).Child("Final Grade").Value.ToString();
                   UIstock.text = e2.Snapshot.Child(cardNames[currentCard]).Child("Stock").Value.ToString();

                   //******Delete Screen******
                   //if so, write the shirt name to the Delete screen in the shirt title field
                   BIname.text = cards.Key.ToString();

                   //set the text for each categor in the Delete scene view
                   BIcentering.text = "Centering: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Centering").Value.ToString();
                   BIcorners.text = "Corners: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Corners").Value.ToString();
                   BIedges.text = "Edges: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Edges").Value.ToString();
                   BIsurface.text = "Surface: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Surface").Value.ToString();
                   BIfinalGrade.text = "Final Grade: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Final Grade").Value.ToString();
                   BIstock.text = "Stock: " + e2.Snapshot.Child(cardNames[currentCard]).Child("Stock").Value.ToString();

                   //Button check
                   //buy button check
                   //turns on or off the button on restarts based on inventory
                   if (int.Parse(e2.Snapshot.Child(cardNames[currentCard]).Child("Stock").Value.ToString()) <= 0)
                   {
                       buyCard.gameObject.SetActive(false);
                   }
                   else
                   {
                       buyCard.gameObject.SetActive(true);
                   }
               }
           }

           //set the variable firstload to true so the code in the update method will run
           firstLoad = true;
       };
    }

    // Update is called once per frame
    void Update()
    {
        //is firebase initialized and is this the first time loaded
        if (isFirebaseInitialized && firstLoad) 
        {
            //get the needed data
            foreach (DataSnapshot cards in publicData.Children)
            {
                //get the value out of the snapshot - IDictionary is data type to cast to
                IDictionary myData = (IDictionary) cards.Value;

                //checked to see if the shirt in the datasnapshot is the same as the current card in our list
                if (cards.Key.ToString().Equals(cardNames[currentCard]))
                {
                    //create download url ref to the file
                    StorageReference download_ref = images_ref.Child(myData["Picture"].ToString());

                    //fetch download url
                    download_ref.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) =>
                    {
                        //no errors
                        if (!task.IsFaulted && !task.IsCanceled)
                        {
                            //set url vars
                            imageURL = task.Result.ToString();

                            //set image bool to true
                            redrawCardImage = true;                        }
                        else
                        {
                            //there was an error
                            Debug.Log(task.Exception);
                        }
                    });
                }
            }

            //set var to false so the update code doesn't run again
            firstLoad = false;

        }

        //load card texture from the web
        if (redrawCardImage)
        {
            //start coroutine to download the bgraphic
            StartCoroutine(GetTextures(imageURL));

            //set the car redrawCardImage to true so the code in the update method will run
            redrawCardImage = false;
        }
    }

    //coroutine to run in the background downloading the card image passed to it
    IEnumerator GetTextures(string url)
    {
        //request to get the texture from the fiven url
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        //wait until the request is processed
        yield return www.SendWebRequest();

        //if else error check and update shirt texture
        //if (www.ConnectionError || www.ProtocolError)? 
        if (www.isNetworkError || www.isHttpError) //throwing decprecation warnings      
        {
            Debug.Log(www.error);
        }
        else
        {
            //update view card texture
            VIcardsImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            //update update card texture
            UIcardsImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            //update delete card texture
            BIcardsImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    //public method to show the next card
    public void NextCard()
    {
        //increase currentcard by 1
        currentCard++;

        if (currentCard > numberOfCards - 1)
        {
            //if so set to 0
            currentCard = 0;
        }

        //update view card texture
        VIcardsImage.texture = loadingImage;

        //update update card texture
        UIcardsImage.texture = loadingImage;

        //update delete card texture
        BIcardsImage.texture = loadingImage;

        //load the next card
        ChangeCard();
    }

    //public method to show the previous card
    public void PreviousCard()
    {
        //decrease currentcard by 1
        currentCard--;

        if (currentCard < 0)
        {
            //if so set to 0
            currentCard = numberOfCards - 1;
        }

        //update view card texture
        VIcardsImage.texture = loadingImage;

        //update update card texture
        UIcardsImage.texture = loadingImage;

        //update delete card texture
        BIcardsImage.texture = loadingImage;

        //load the next card
        ChangeCard();
    }

    public void ChangeCard()
    {
        //cycle through the data in the data snapshot publicData
        foreach (DataSnapshot cards in publicData.Children) 
        {
            //get the vvalue out of the snapshot - IDictionary is data type to case
            IDictionary myData = (IDictionary)cards.Value;

            //check to see if the shoirt in the datasnapshot is the same as the current shirt in our list
            if (cards.Key.ToString().Equals(cardNames[currentCard]))
            {
                //Button check
                //buy button check
                //turns on or off the button on restarts based on inventory
                if (int.Parse(myData["Stock"].ToString()) <= 0)
                {
                    buyCard.gameObject.SetActive(false);
                }
                else
                {
                    buyCard.gameObject.SetActive(true);
                }

                //***********View Screen********
                //if so, write the card name to the view screen in the card names field
                VIname.text = cards.Key.ToString();

                //set the text for each category in the view scene view
                VIcentering.text = "Centering: " + myData["Centering"].ToString();
                VIcorners.text = "Corners: " + myData["Corners"].ToString();
                VIedges.text = "Edges: " + myData["Edges"].ToString();
                VIsurface.text = "Surface: " + myData["Surface"].ToString();
                VIfinalGrade.text = "Final Grade: " + myData["Final Grade"].ToString();
                VIstock.text = "Stock: " + myData["Stock"].ToString();

                //***********Update Screen********
                //if so, write the card name to the update screen in the card names field
                UIname.text = cards.Key.ToString();

                //set the text for each category in the update scene view
                UIcentering.text = myData["Centering"].ToString();
                UIcorners.text = myData["Corners"].ToString();
                UIedges.text = myData["Edges"].ToString();
                UIsurface.text = myData["Surface"].ToString();
                UIfinalGrade.text = myData["Final Grade"].ToString();
                UIstock.text = myData["Stock"].ToString();

                //***********Delete Screen********
                //if so, write the card name to the delete screen in the card names field
                BIname.text = cards.Key.ToString();

                //set the text for each category in the delete scene view
                BIcentering.text = "Centering: " + myData["Centering"].ToString();
                BIcorners.text = "Corners: " + myData["Corners"].ToString();
                BIedges.text = "Edges: " + myData["Edges"].ToString();
                BIsurface.text = "Surface: " + myData["Surface"].ToString();
                BIfinalGrade.text = "Final Grade: " + myData["Final Grade"].ToString();
                BIstock.text = "Stock: " + myData["Stock"].ToString();

                //create download url ref to the file
                StorageReference download_ref = images_ref.Child(myData["Picture"].ToString());

                //fetch download url
                download_ref.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) => 
                {
                    //if no errors
                    if (!task.IsFaulted && !task.IsCanceled) 
                    {
                        //set url vars
                        imageURL = task.Result.ToString();

                        //set image bool to true
                        redrawCardImage = true;
                    }
                    else
                    {
                        Debug.Log(task.Exception);
                    }
                });
            }
        }
    }

    // update data
    public void updateCardData() //write to database on server
    {
        //create dictionary to hold all 6 pieces of the data in one obj
        Dictionary<string, object> dataToWriteUpdate = new Dictionary<string, object>();

        //put each piece of the 6 data in one obj
        dataToWriteUpdate["Centering"] = UIcentering.text.ToString();
        dataToWriteUpdate["Corners"] = UIcorners.text.ToString();
        dataToWriteUpdate["Edges"] = UIedges.text.ToString();
        dataToWriteUpdate["Surface"] = UIsurface.text.ToString();
        dataToWriteUpdate["Final Grade"] = UIfinalGrade.text.ToString();
        dataToWriteUpdate["Stock"] = UIstock.text.ToString();

        //write the one obj to the firebase db
        cardsRef.Child(cardNames[currentCard]).UpdateChildrenAsync(dataToWriteUpdate);
    }

    //buy cards method
    public void buyCardData(string cardStock)
    {
        //create dictionary to hold all 6 pieces of the data in one obj
        Dictionary<string, object> dataToWriteBuy = new Dictionary<string, object>();

        //reset the inventory value to 0
        int newInventoryValue = 0;

        //check to see which card is being purchased
        if (cardStock == "Stock")
        {
            //grab the card text information from it's text field
            string cardText = BIstock.text.ToString();

            //set up data to be removed
            string whatToRemove = "Stock: ";

            //remove the un-needed data
            string result = cardText.Replace(whatToRemove, "");

            //if no more cards to purchase 
            if (int.Parse(result) <= 0)
            {
                Debug.Log("No More " + cardStock + " cards");
                return;
            }
            //subtract 1 from current inventory
            newInventoryValue = int.Parse(result) - 1;

            //if this new value is equal to zero turn off buy button
            if (newInventoryValue == 0) 
            {
                buyCard.gameObject.SetActive(false);
            }
        }
        
        //create the object to hold the changing card stock value
        dataToWriteBuy[cardStock] = newInventoryValue;

        //write the one object to the firebase db
        cardsRef.Child(cardNames[currentCard]).UpdateChildrenAsync(dataToWriteBuy);
    }
}
