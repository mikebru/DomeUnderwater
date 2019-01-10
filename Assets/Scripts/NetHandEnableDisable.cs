using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

using Leap.Unity;

public class NetHandEnableDisable : NetworkBehaviour
{
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {


            Debug.Log("Added local player events");
        }
    }


}