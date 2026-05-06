using UnityEngine;
using Whisper;

public class WhisperTest : MonoBehaviour
{
    public WhisperManager whisper;

    private AudioClip clip;
    private string mic;

    void Start()
    {
        mic = Microphone.devices[0];
        Debug.Log("Mic ready: " + mic);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            clip = Microphone.Start(mic, false, 5, 16000);
            Debug.Log("Recording...");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Microphone.End(mic);
            Debug.Log("Transcribing...");
            StartCoroutine(RunWhisper());
        }
    }

    System.Collections.IEnumerator RunWhisper()
    {
        var task = whisper.GetTextAsync(clip);
        yield return new WaitUntil(() => task.IsCompleted);

        var result = task.Result;

        if (result == null)
        {
            Debug.Log("No result returned from Whisper");
            yield break;
        }

        string fullText = "";

        foreach (var segment in result.Segments)
        {
            fullText += segment.Text + " ";
        }

        Debug.Log("RESULT: " + fullText);
    }
}