using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.IO;
using System;

public class VoiceAnalyzer : MonoBehaviour
{
    public string microphoneName;
    private AudioClip audioClip;
    private int sampleWindow = 128;
    private AudioSource audioSource;
    public TextMeshProUGUI loudnessText;
    private string apiUrl = "https://web-production-caf4b.up.railway.app/analyze";

    void Start()
    {
        loudnessText = GameObject.Find("LoudnessText").GetComponent<TextMeshProUGUI>();

        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0];
            StartCoroutine(RecordAndAnalyze());
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
    }

    IEnumerator RecordAndAnalyze()
    {
        // Start recording (10 seconds)
        audioClip = Microphone.Start(microphoneName, false, 10, 44100);
        Debug.Log("Recording started...");
        yield return new WaitForSeconds(10);
        Microphone.End(microphoneName);
        Debug.Log("Recording stopped.");

        // Upload the recorded audio
        StartCoroutine(UploadAudio(audioClip));
    }

    IEnumerator UploadAudio(AudioClip clip)
    {
        byte[] audioData = ConvertAudioClipToWAV(clip); // Convert to WAV

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, "user_audio.wav", "audio/wav");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API Response: " + webRequest.downloadHandler.text);
                loudnessText.text = "Loudness: " + webRequest.downloadHandler.text; // Update UI with API response
            }
            else
            {
                Debug.LogError("Error: " + webRequest.error);
                loudnessText.text = "Error analyzing voice";
            }
        }
    }

    private byte[] ConvertAudioClipToWAV(AudioClip clip)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        int sampleCount = clip.samples * clip.channels;
        float[] samples = new float[sampleCount];
        clip.GetData(samples, 0);

        short[] intData = new short[sampleCount];
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * short.MaxValue);
        }

        byte[] byteData = new byte[intData.Length * 2];
        Buffer.BlockCopy(intData, 0, byteData, 0, byteData.Length);

        writer.Write(new char[] { 'R', 'I', 'F', 'F' });
        writer.Write(36 + byteData.Length);
        writer.Write(new char[] { 'W', 'A', 'V', 'E' });
        writer.Write(new char[] { 'f', 'm', 't', ' ' });
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)clip.channels);
        writer.Write(clip.frequency);
        writer.Write(clip.frequency * clip.channels * 2);
        writer.Write((short)(clip.channels * 2));
        writer.Write((short)16);
        writer.Write(new char[] { 'd', 'a', 't', 'a' });
        writer.Write(byteData.Length);
        writer.Write(byteData);

        writer.Flush();
        writer.Close();
        return stream.ToArray();
    }
}
