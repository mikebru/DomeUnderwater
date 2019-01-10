using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Networking;

public class EnableEventRelay : MonoBehaviour
{
    public UnityEvent Enabled;
    public UnityEvent Disabled;

    void OnEnable()
    {
       // if (transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
       // {
            Enabled.Invoke();
        //}
    }

    void OnDisable()
    {
        //if (transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
      //  {
            Disabled.Invoke();
       // }
    }
}
