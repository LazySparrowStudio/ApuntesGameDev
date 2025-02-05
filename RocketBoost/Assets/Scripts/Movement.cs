using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;

    [SerializeField] float thrustStrengh = 10;
    [SerializeField] float rotationStrengh = 10;
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem LeftBooster;
    [SerializeField] ParticleSystem RightBooster;

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
        if (thrust.IsPressed())
        {
            // rb.AddRelativeForce(0,10,0);
            rb.AddRelativeForce(Vector3.up * thrustStrengh * Time.fixedDeltaTime);
           
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngineSFX);
            
            }
            if(!mainBooster.isPlaying)
            {
                mainBooster.Play();
            }

        else{
            audioSource.Stop();
           /* mainBooster.Stop();
            */
        }
        }
    }

    private void ProcessRotation()
    {
        if (rotation.IsPressed())
        {
            
            float rotationInput = rotation.ReadValue<float>();
            if(rotationInput > 0)
            {
                //transform.Rotate(0,0,1f);
                ////transform.Rotate(Vector3.forward * rotationStrengh * Time.fixedDeltaTime);
                ApplyRotation(rotationStrengh);
                if(!RightBooster.isPlaying)
                    {
                        LeftBooster.Stop(); 
                        RightBooster.Play();
                    }
            }

            if (rotationInput < 0)
            {
                //transform.Rotate(0,0,-1f);
                //transform.Rotate(Vector3.back * rotationStrengh * Time.fixedDeltaTime);
                ApplyRotation(-rotationStrengh);
                if(!mainBooster.isPlaying)
                    {
                        RightBooster.Stop();
                        LeftBooster.Play();
                    }
            }
          //  Debug.Log("Rotation value: "+ rotationInput);

        }else
        {
            RightBooster.Stop();
            LeftBooster.Stop(); 
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
        if(thrust.IsPressed())
        {

            Debug.Log("FIIIIIIIOOOM");
        }
    }

}

