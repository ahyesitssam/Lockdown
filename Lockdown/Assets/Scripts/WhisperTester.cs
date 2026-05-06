using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using Whisper;
using System.Collections;

public class RecordAudio : MonoBehaviour
{
    [SerializeField] private CommandManager TM;
    [SerializeField] private UIManager UI;
    [SerializeField] private Camera xrCamera;
    [SerializeField] private WhisperManager whisper;

    private GameObject currentTarget;
    private AudioClip clip;
    private string mic;
    private bool isRecording = false;

    void Start()
    {
        foreach (var device in Microphone.devices)
        {
            if (device.Contains("Oculus"))
            {
                mic = device;
            }
        }
        Debug.Log("Mic ready: " + mic);
    }

    private void DoRaycast()
    {
        Ray ray = new Ray(xrCamera.transform.position, xrCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            currentTarget = hit.collider.gameObject;
            Debug.Log("Hit object: " + currentTarget.name + " | Tag: " + currentTarget.tag);
        }
        else
        {
            currentTarget = null;
            Debug.Log("No object hit");
        }
    }

    public void OnGrip(InputAction.CallbackContext context)
    {
        if (context.performed && !isRecording)
        {
            DoRaycast();
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

    private IEnumerator RunWhisper()
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
            UI.UpdateRecentCommand(fullText);
            TM.ProcessText(fullText, currentTarget);
        } else
        {
            Debug.LogError("WhisperTester does not have ref for TerminalManager");
        }
        
    }
} // end class