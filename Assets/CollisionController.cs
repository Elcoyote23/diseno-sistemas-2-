using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "slime")
            Debug.Log("enter");
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "slime")
            Debug.Log("stay");
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "slime")
            Debug.Log("exit");
    }
}
