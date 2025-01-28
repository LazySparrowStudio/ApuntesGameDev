using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter (Collision other)
    {
        switch (other.gameObject.tag )
        {
         case "Friendly":
         
            Debug.Log ("La chupas contra " + other.gameObject.tag);
         break;
         
         case "Fuel":
         
            Debug.Log ("La chupas contra " + other.gameObject.tag);
         break;

         case "Finish" :
         
            Debug.Log ("La chupas contra " + other.gameObject.tag);
         break;

         default:

            Debug.Log("Bum Cacha Bum");
            break;
        }
    }
}
