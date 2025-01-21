using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    Rigidbody rb;

    private void OnEnable()
    {
        thrust.Enable();
    }

     private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(thrust.isPressed(true))
        {
            
           rb.AddRelativeForce();
        }
    }

    private void Update()
    {
        if(thrust.isPressed(true))
        {

            Debug.Log("FIIIIIIIOOOM");
        }
    }
}

/* Lista de cosas que hay que hacer en casa por el cambio de version a unity 6
Script movement - Add binding --> Space Bar

*/