using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CollisionHandler : MonoBehaviour
{
   [SerializeField] float delayTime = 2f;
   [SerializeField] AudioClip successSFX;
   [SerializeField] AudioClip crashSFX;
   [SerializeField] ParticleSystem successParticles;
   [SerializeField] ParticleSystem crashParticles;
   [SerializeField] InputAction nextLevelKey;
   AudioSource audioSource;
  // ParticleSystem particleSystem;

   bool isControllable = true;
   bool isCollidable = true;

   private void Start()
   {
      isCollidable = true;
      isControllable = true;
      audioSource = GetComponent<AudioSource>();   
   }

   private void Update()
   {
      RespondToDebugKeys();
   }

    private void RespondToDebugKeys()
    {
      if (Keyboard.current.lKey.wasPressedThisFrame) 
      {
         LoadNextLevel();
         
      }else if(Keyboard.current.cKey.wasPressedThisFrame)
      {
         isCollidable = !isCollidable;

      }else if(Keyboard.current.escapeKey.wasPressedThisFrame)
      {
         Application.Quit();
         Debug.Log("Escape pushed");
      }
      
      
    }

    private void OnCollisionEnter (Collision other)
    {
      if (!isControllable || !isCollidable) {return;}
         
        switch (other.gameObject.tag )
        {
         case "Friendly":
            Debug.Log ("La chupas contra " + other.gameObject.tag);
         break;
         
         case "Fuel":
            Debug.Log ("La chupas contra " + other.gameObject.tag);
         break;

         case "Finish" :
            startSuccessSequence();
            Invoke("LoadNextLevel", delayTime);
            Debug.Log ("La chupas contra " + other.gameObject.tag);
         break;

         case "Floor" :
            Debug.Log ("La chupas contra " + other.gameObject.tag);
         break;

         default:
            StartCrashSequence();
            Debug.Log("La chupas contra " +other.gameObject.name);
         break;
        }
      }
    
   private void startSuccessSequence()
    {
      audioSource.Stop();
      audioSource.PlayOneShot (successSFX);
      successParticles.Play();
      GetComponent<Movement>().enabled = false;
      Invoke("LoadNextLevel", delayTime);
      isControllable=false;
    }

   void StartCrashSequence()
    {
      audioSource.Stop();
      audioSource.PlayOneShot (crashSFX);
      crashParticles.Play();
      GetComponent<Movement>().enabled = false;
      Invoke("ReloadLevel", delayTime);
      Debug.Log("INICIADO REBOOT");
      isControllable=false;
        
    }

   void LoadNextLevel()
        {
            GetComponent<Movement>().enabled = false;
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextScene = currentScene + 1;

            if(nextScene == SceneManager.sceneCountInBuildSettings)
            {
               nextScene = 0;
            }
             SceneManager.LoadScene(nextScene);
        }

   void ReloadLevel()
        {
             int currentScene = SceneManager.GetActiveScene().buildIndex;
             SceneManager.LoadScene(currentScene);
        }

}
