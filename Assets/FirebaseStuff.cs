using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Firebase.Database;
using System.Linq;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
public class FirebaseStuff : MonoBehaviour
{
    public DatabaseReference dbReference;  //firebase
    public AllVariables allVariables;


    // Start is called before the first frame update
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator LogDetails(string details, string key)
    {



        var DBtask = dbReference.Child("LogInfo").Child(allVariables.participantName).Child(key).SetValueAsync(details);
        yield return new WaitUntil(predicate: () => DBtask.IsCompleted);

    }

}
