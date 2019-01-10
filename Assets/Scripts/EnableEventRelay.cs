using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnableEventRelay : MonoBehaviour
{
    public UnityEvent Enabled;
    public UnityEvent Disabled;

    void OnEnable()
    {
        Enabled.Invoke();
    }

    void OnDisable()
    {
        Disabled.Invoke();
    }
}
