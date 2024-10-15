using UnityEngine;
using Firebase.Database;
using System.Collections;
using System;
using UnityEditor;

public class DatabaseManager : SingletonMonoBehaviour<DatabaseManager>
{
    private DatabaseReference reference;
    [SerializeField] private float databaseConnectionTime = 1;

    /*public int participantId;
    public string pointingTechnique, pointerType, confirmationTechnique;
    public bool isRecord;*/

    void Start()
    {
        Firebase.AppOptions options = new Firebase.AppOptions();
        options.ApiKey = "AIzaSyC73AF2_hEIhBeBJFDElb2y7n3wW5yMZVI";
        options.AppId = "1:523616341490:android:59a87bf752c13d9c096f6b";
        options.DatabaseUrl = new Uri("https://ismar2024-multimodality-default-rtdb.firebaseio.com");
        options.ProjectId = "ismar2024-multimodality";
        options.StorageBucket = "ismar2024-multimodality.appspot.com";

        var app = Firebase.FirebaseApp.Create(options);
        reference = FirebaseDatabase.GetInstance(app).RootReference;

        //StartCoroutine(GetSettingsFirstTime());
    }

    private IEnumerator GetParticipantId(Action<int> onCallback)
    {
        var participantIdData = reference.Child("settings").Child("participantId").GetValueAsync();
        yield return new WaitUntil(predicate: () => participantIdData.IsCompleted);
        DataSnapshot snapshot = participantIdData.Result;
        var value = snapshot.Value;
        if (value != null) onCallback.Invoke(int.Parse(value.ToString()));
        else onCallback.Invoke(-1);
    }

    private IEnumerator GetPointingTechnique(Action<string> onCallback)
    {
        var pointingTechniqueData = reference.Child("settings").Child("pointingTechnique").GetValueAsync();
        yield return new WaitUntil(predicate: () => pointingTechniqueData.IsCompleted);
        DataSnapshot snapshot = pointingTechniqueData.Result;
        var value = snapshot.Value;
        if (value != null) onCallback.Invoke(value.ToString());
        else onCallback.Invoke(null);
    }

    private IEnumerator GetPointerType(Action<string> onCallback)
    {
        var pointerTypeData = reference.Child("settings").Child("pointerType").GetValueAsync();
        yield return new WaitUntil(predicate: () => pointerTypeData.IsCompleted);
        DataSnapshot snapshot = pointerTypeData.Result;
        var value = snapshot.Value;
        if (value != null) onCallback.Invoke(value.ToString());
        else onCallback.Invoke(null);
    }

    private IEnumerator GetConfirmationTechnique(Action<string> onCallback)
    {
        var confirmationTechniqueData = reference.Child("settings").Child("confirmationTechnique").GetValueAsync();
        yield return new WaitUntil(predicate: () => confirmationTechniqueData.IsCompleted);
        DataSnapshot snapshot = confirmationTechniqueData.Result;
        var value = snapshot.Value;
        if (value != null) onCallback.Invoke(value.ToString());
        else onCallback.Invoke(null);
    }

    private IEnumerator GetIsRecord(Action<bool> onCallback)
    {
        var isRecordData = reference.Child("settings").Child("record").GetValueAsync();
        yield return new WaitUntil(predicate: () => isRecordData.IsCompleted);
        DataSnapshot snapshot = isRecordData.Result;
        var value = snapshot.Value;
        if (value != null) onCallback.Invoke(bool.Parse(value.ToString()));
        else onCallback.Invoke(false);
    }

    private void GetSettings()
    {
        /*StartCoroutine(GetParticipantId((int participantId) => { this.participantId = participantId; }));
        StartCoroutine(GetPointingTechnique((string pointingTechnique) => { this.pointingTechnique = pointingTechnique.ToLower(); }));
        StartCoroutine(GetPointerType((string pointerType) => { this.pointerType = pointerType.ToLower(); }));
        StartCoroutine(GetConfirmationTechnique((string confirmationTechnique) => { this.confirmationTechnique = confirmationTechnique.ToLower(); }));
        StartCoroutine(GetIsRecord((bool isRecord) => { this.isRecord = isRecord; }));*/
    }

    private IEnumerator GetSettingsFirstTime()
    {
        TeleportationManager TM = TeleportationManager.Instance;
        TM.gameObject.SetActive(false);
        GameObject PR = TM.pointingRay.gameObject;
        PR.SetActive(false);
        GetSettings();
        yield return new WaitForSeconds(databaseConnectionTime);
        //GameManager.Instance.GetData();
        yield return new WaitForSeconds(databaseConnectionTime);
        TM.gameObject.SetActive(true);
        PR.SetActive(true);
        CheckpointManager.Instance.StartingProcedure();
        TeleportationManager.Instance.StartingProcedure();
    }

    public IEnumerator LogStudyData(StudyDataLog log)
    {
        string json = JsonUtility.ToJson(log);
        string ts = log.timestamp.ToString().Replace("/", "-");
        var DBTask = reference.Child("studyLogs").Child("participant_" + log.participantId).
            Child(log.pointingTechniqueCombinedName).Child(log.confirmationTechnique).
            Child(ts).SetRawJsonValueAsync(json);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
}