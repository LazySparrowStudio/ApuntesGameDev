using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

/* Lista de cosas que hay que hacer en casa por el cambio de version a unity 6
Script movement - thrust  - Add binding --> Xaxis
Script movement - rotation  - Add binding --> Positive/Negative binding --> Left arrow for negative // Right arrow for positive
- Vuelvete a ver la leccion de Cinemachine Follow Camera por diferencias de version
- Audio Source tiene el primer campo como "AudioClip" y en U6 es Audio Resoruce, ojo con eso
- El SceneManager basicamente no existe en U2023, asi que hay que volver a verse el UsingSceneManager
*/

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;

    [SerializeField] float thrustStrengh = 10;
    [SerializeField] float rotationStrengh = 10;
    Rigidbody rb;
    AudioSource audioSource;

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

     private void Start()
    {
        audioSource = GetComponent<AudioSource>();    
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.isPressed(true))
        {

            // rb.AddRelativeForce(0,10,0);
            rb.AddRelativeForce(Vector3.up * thrustStrengh * Time.fixedDeltaTime);
            if(!audioSource.isPlaying)
            {

            audioSource.Play();

            }

        }else
        {
            audioSource.Stop();
        }
    }

    private void ProcessRotation()
    {
        if (rotation.isPressed(true))
        {
            
            float rotationInput = rotation.ReadValue<float>();
            if(rotationInput < 0)
            {
                //transform.Rotate(0,0,1f);
                ////transform.Rotate(Vector3.forward * rotationStrengh * Time.fixedDeltaTime);
                ApplyRotation(rotationStrengh);
            }

            if (rotationInput < 0)
            {
                //transform.Rotate(0,0,-1f);
                //transform.Rotate(Vector3.back * rotationStrengh * Time.fixedDeltaTime);
                ApplyRotation(-rotationStrengh);
            }
          //  Debug.Log("Rotation value: "+ rotationInput);

        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

    private void Update()
    {
        if(thrust.isPressed(true))
        {

            Debug.Log("FIIIIIIIOOOM");
        }
    }
}

