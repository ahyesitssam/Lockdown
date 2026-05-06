using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using Whisper;

public class WhisperTest : MonoBehaviour
{
    [SerializeField] private TerminalManager TM;
    public WhisperManager whisper;

    private AudioClip clip;
    private string mic;
    private bool isRecording = false;

    void Start()
    {
        mic = Microphone.devices[0];
        Debug.Log("Mic ready: " + mic);
    }

    public void OnGrip(InputAction.CallbackContext context)
    {
        if (context.performed && !isRecording)
        {
            isRecording = true;
            clip = Microphone.Start(mic, false, 5, 16000);
            Debug.Log("Recording...");
        }

        if (context.canceled && isRecording)
        {
            isRecording = false;
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
        
        if (TM != null)
        {
            TM.ProcessText(fullText);
        } else
        {
            Debug.LogError("WhisperTester does not have ref for TerminalManager");
        }
        
    }
} // end class