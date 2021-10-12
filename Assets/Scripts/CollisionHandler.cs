using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    Movement movementScript;
    Debugger debugger;
    AudioSource rocketAudioSource;
    GameManager gameManager;
    [SerializeField] float delayTime = 1.0f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem explosionLightParticle;
    [SerializeField] ParticleSystem explosionSparkParticle;
    [SerializeField] ParticleSystem explosionSmokeParticle;
    [SerializeField] ParticleSystem successParticle;

    bool isTransitioning = false;
    private void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();
        debugger = gameManager.GetComponentInChildren<Debugger>();
        movementScript = GetComponent<Movement>();
        rocketAudioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other) 
    {
        if(isTransitioning || debugger.GetCollisionStatus())
        {
            return;
        }
        switch(other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "LandingPad":
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
        rocketAudioSource.PlayOneShot(crashSound);
        PlayExplosionParticleEffect();
        isTransitioning = true;
        Invoke("ReloadLevel", delayTime);
    }
    void ImplementSuccessSequence()
    {
        movementScript.enabled = false;
        if(rocketAudioSource.isPlaying)
            rocketAudioSource.Stop();
        rocketAudioSource.PlayOneShot(successSound);
        successParticle.Play();
        isTransitioning = true;
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
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void PlayExplosionParticleEffect()
    {
        explosionLightParticle.Play();
        explosionSmokeParticle.Play();
        explosionSparkParticle.Play();
    }
}
