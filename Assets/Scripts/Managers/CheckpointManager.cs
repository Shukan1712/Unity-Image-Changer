using ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : SingletonMonoBehaviour<CheckpointManager>
{
    [SerializeField] private GameObject checkpointPrefab;
    [SerializeField] public int checkpointQuantity;
    public List<GameObject> checkpoints;
    public List<float> heights, distances, sizes, angles;
    public List<int> repeatNumbers;
    public Target currentTarget;
    private Vector3 spawnPosition = Vector3.zero;
    private Vector3 checkpointScale = Vector3.one;
    public int current = -1;
    private List<(float height, float distance, float targetSize)> _dhCombosBalanced;
    private Dictionary<(float height, float distance), int> comboRepeatNumbers = new Dictionary<(float height, float distance), int>();


    public void StartingProcedure()
    {
        ResetData();
        GenerateCheckpoints();
        HideCheckpoints();
        DisplayNextCheckpoint();
    }

    private void ResetData()
    {
        Destroy(GameObject.FindGameObjectWithTag("Start"));
        for (int i = 0; i < checkpoints.Count; i++)
        {
            Destroy(checkpoints[i]);
            checkpoints[i] = null;
        }
        checkpoints = new List<GameObject>();
        heights = new List<float>();
        distances = new List<float>();
        sizes = new List<float>();
        angles = new List<float>();
        repeatNumbers = new List<int>();
        spawnPosition = Vector3.zero;
        checkpointScale = Vector3.one;
        current = -1;
        _dhCombosBalanced = new List<(float height, float distance, float targetSize)>();
        comboRepeatNumbers = new Dictionary<(float height, float distance), int>();
    }

    private void GenerateCheckpoints()
    {
        IEnumerable<(float height, float distance, float targetSize)> dhCombos;

        float[] heightSet = { 3, 0, -3 };
        float[] distanceSet = { 12, 24 };
        float[] targetSizeSet = { 1 };
        int repeatTimes = 5;

        /*float[] heightSet = { 9, 0, -9 };
        float[] distanceSet = { 20, 40, 60 };
        float[] targetSizeSet = { 1 };
        int repeatTimes = 5;*/

        dhCombos = (
            from h in heightSet
            from d in distanceSet
            from ts in targetSizeSet
            select (height: h, distance: d, targetSize: ts)
        );

        List<(float height, float distance, float targetSize)> _dhCombos = new List<(float height, float distance, float targetSize)>();

        for (int i = 0; i < repeatTimes; i++)
        {
            _dhCombos.AddRange(dhCombos.ToList());
        }

        _dhCombos.Shuffle();

        List<(float height, float distance, float targetSize)> _dhCombosZero = new List<(float height, float distance, float targetSize)>();
        List<(float height, float distance, float targetSize)> _dhCombosNonZero = new List<(float height, float distance, float targetSize)>();

        foreach ((float height, float distance, float targetSize) item in _dhCombos)
        {
            if (item.height == 0)
            {
                _dhCombosZero.Add(item);
            }
            else
            {
                _dhCombosNonZero.Add(item);
            }
        }

        _dhCombosBalanced = new List<(float height, float distance, float targetSize)>();


        while (_dhCombosZero.Count > 0)
        {
            _dhCombosBalanced.Add(_dhCombosZero.First());
            _dhCombosZero.RemoveAt(0);

            _dhCombosBalanced.Add(_dhCombosNonZero.First());
            _dhCombosNonZero.RemoveAt(0);
        }

        while (_dhCombosNonZero.Count > 0)
        {
            int index = Random.Range(0, _dhCombosBalanced.Count);
            if (_dhCombosBalanced[index].height == 0)
            {
                _dhCombosBalanced.Insert(index, _dhCombosNonZero.First());
                _dhCombosNonZero.RemoveAt(0);
            }
        }

        /*float[] practiceDistances = { 20, 20, 40, 40, 60 };
        float[] practiceHeights = { 0, 9, 0, -9, 9 };
        float[] practiceAngles = { 0, 0, Mathf.PI / 12, -Mathf.PI / 12, Mathf.PI / 12 };*/

        float[] practiceDistances = { 12, 12, 24, 24, 24 };
        float[] practiceHeights = { 0, 3, 0, -3, 3 };
        float[] practiceAngles = { 0, 0, Mathf.PI / 12, -Mathf.PI / 12, Mathf.PI / 12 };

        for (int i = 0; i < practiceDistances.Length; i++)
        {
            spawnPosition.z += practiceDistances[i];
            spawnPosition.y += practiceHeights[i];
            float angle = practiceAngles[i];
            spawnPosition.x += Mathf.Tan(angle) * practiceDistances[i];

            GameObject checkpoint = Instantiate(checkpointPrefab, spawnPosition, Quaternion.identity, transform);
            checkpoints.Add(checkpoint);
            repeatNumbers.Add(0);
            distances.Add(practiceDistances[i]);
            heights.Add(practiceHeights[i]);
            sizes.Add(1);
            angles.Add(angle);
        }

        for (int i = 0; i < _dhCombosBalanced.Count(); i++)
        {   
            spawnPosition.z += _dhCombosBalanced[i].distance;
            spawnPosition.y += _dhCombosBalanced[i].height;
            float angle = Random.Range(0, 2) * (Mathf.PI / 12) * (2 * (i % 2) - 1);
            spawnPosition.x += Mathf.Tan(angle) * _dhCombosBalanced[i].distance;
            checkpointScale = Vector3.one * _dhCombosBalanced[i].targetSize;

            GameObject checkpoint = Instantiate(checkpointPrefab, spawnPosition, Quaternion.identity, transform);
            checkpoint.transform.localScale = checkpointScale;
            checkpoints.Add(checkpoint);
            angles.Add(angle);
            distances.Add(_dhCombosBalanced[i].distance);
            heights.Add(_dhCombosBalanced[i].height);
            sizes.Add(1);

            if (!comboRepeatNumbers.ContainsKey((_dhCombosBalanced[i].height, _dhCombosBalanced[i].distance)))
            {
                comboRepeatNumbers[(_dhCombosBalanced[i].height, _dhCombosBalanced[i].distance)] = 1;
            }
            else
            {
                comboRepeatNumbers[(_dhCombosBalanced[i].height, _dhCombosBalanced[i].distance)]++;
            }

            repeatNumbers.Add(comboRepeatNumbers[(_dhCombosBalanced[i].height, _dhCombosBalanced[i].distance)]);
        }

        checkpointQuantity = checkpoints.Count();
    }

    private void HideCheckpoints()
    {
        foreach (GameObject checkpoint in checkpoints) 
            checkpoint.gameObject.SetActive(false);
    }

    public void HideCurrentCheckpoint()
    {
        currentTarget.gameObject.SetActive(false);
        checkpoints[current].transform.Find("Platform").gameObject.transform.position += new Vector3(0, -1, 0);
    }

    public void DisplayNextCheckpoint()
    {
        current++;
        if (current != checkpointQuantity)
        {
            checkpoints[current].gameObject.SetActive(true);
            currentTarget = checkpoints[current].transform.Find("Target").GetComponent<Target>();
        }
    }
}