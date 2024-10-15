using UnityEngine;

public class VoiceDetection : ConfirmationDetection
{
    [SerializeField]
    private int sampleWindow = 64;

    [SerializeField]
    private float loudnessSensitivity = 100;

    [SerializeField]
    private float highThreshold = 2.0f, lowThreshold = 0.1f;

    private AudioClip microphoneClip;
    private bool isHigh = false;

    private void Start()
    {
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public override bool ConfirmationDetected()
    {
        float loudness = GetLoudnessFromMicrophone() * loudnessSensitivity;

        if (loudness < lowThreshold)
        {
            if (isHigh)
            {
                isHigh = false;
                return true;
            }
        }
        else if (loudness > highThreshold)
        {
            isHigh = true;
        }

        return false;
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if(startPosition < 0) return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
