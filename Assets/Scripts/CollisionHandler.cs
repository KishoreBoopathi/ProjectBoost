using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    Movement movementScript;
    AudioSource rocketAudioSource;
    [SerializeField] float delayTime = 1.0f;

    private void Start() 
    {
        movementScript = GetComponent<Movement>();
        rocketAudioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other) 
    {
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("No worries. Keep going");
                break;
            case "LandingPad":
                Debug.Log("Nailed the landing.");
                ImplementSuccessSequence();
                
                break;
            default:
                ImplementCrashSequence();
                break;
        }    
    }
    void ImplementCrashSequence()
    {
        movementScript.enabled = false;
        if(rocketAudioSource.isPlaying)
            rocketAudioSource.Stop();
        Invoke("ReloadLevel", delayTime);
    }
    void ImplementSuccessSequence()
    {
        movementScript.enabled = false;
        if(rocketAudioSource.isPlaying)
            rocketAudioSource.Stop();
        Invoke("LoadNextLevel", delayTime);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            //Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
