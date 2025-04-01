using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Firebase.Database;
using System.Linq;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using Firebase.Extensions;

public class FirebaseStuff : MonoBehaviour
{
    public DatabaseReference dbReference;
    public AllVariables allVariables;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Automatically find AllVariables if not assigned in Inspector
        if (allVariables == null)
        {
            allVariables = FindObjectOfType<AllVariables>();
        }

        // Listen to /Sstate/remoteState changes
        FirebaseDatabase.DefaultInstance.GetReference("Sstate/remoteState")
            .ValueChanged += HandleRemoteStateChanged;
    }

    // Listener for remoteState changes
    void HandleRemoteStateChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("Firebase error: " + args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null && args.Snapshot.Exists)
        {
            int newState = int.Parse(args.Snapshot.Value.ToString());
            allVariables.remoteState = newState;
            Debug.Log("remoteState updated from Firebase: " + newState);
            StartCoroutine(allVariables.experimentController. RunExperiment());
        }
    }

    // Call this method to log participant data to Firebase
    public IEnumerator LogDetails(string details, string key)
    {
        var DBtask = dbReference.Child("LogInfo")
                                .Child(allVariables.participantName)
                                .Child(key)
                                .SetValueAsync(details);

        yield return new WaitUntil(() => DBtask.IsCompleted);

        if (DBtask.Exception != null)
        {
            Debug.LogError("Failed to log details: " + DBtask.Exception);
        }
        else
        {
            Debug.Log("Logged to Firebase: " + key + " = " + details);
        }
    }
}
