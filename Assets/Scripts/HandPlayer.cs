using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HandPlayer : NetworkBehaviour
{
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    EnableEventRelay leftHandEnabler;
    EnableEventRelay rightHandEnabler;

    [Tooltip("Remove these if we're not the player - they'll just get in the way.")]
    [SerializeField] Object[] removeComponentIfNonLocal;
    [SerializeField] GameObject[] removeGameObjectsIfNonLocal;
    [SerializeField] GameObject[] enableGameObjectsIfLocal;

    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < removeGameObjectsIfNonLocal.Length; i++)
            {
                if (removeGameObjectsIfNonLocal[i] == null) continue;
                Destroy(removeGameObjectsIfNonLocal[i]);
            }

            // we need to get rid everything except the raw transforms
            for (int i = 0; i < removeComponentIfNonLocal.Length; i++)
            {
                if (removeComponentIfNonLocal[i] == null) continue;
                Destroy(removeComponentIfNonLocal[i]);
            }

            // specifically removing rigged fingers if non-local
            Leap.Unity.RiggedFinger[] leftHandFingers = leftHand.GetComponentsInChildren
                <Leap.Unity.RiggedFinger>(true);

            Leap.Unity.RiggedFinger[] rightHandFingers = rightHand.GetComponentsInChildren
                <Leap.Unity.RiggedFinger>(true);

            if (leftHandFingers != null)
            {
                for (int i = 0; i < leftHandFingers.Length; i++) Destroy(leftHandFingers[i]);
            }

            if (rightHandFingers != null)
            {
                for (int i = 0; i < rightHandFingers.Length; i++) Destroy(rightHandFingers[i]);
            }

            gameObject.tag = "Untagged";
            gameObject.name = "Player (Remote)";
        }
        else
        {
            gameObject.name = "Player (Local)";

            leftHandEnabler = leftHand.gameObject.AddComponent<EnableEventRelay>();
            leftHandEnabler.Enabled = new UnityEngine.Events.UnityEvent();
            leftHandEnabler.Disabled = new UnityEngine.Events.UnityEvent();
            leftHandEnabler.Enabled.AddListener(CmdLeftHandEnable);
            leftHandEnabler.Disabled.AddListener(CmdLeftHandDisable);

            rightHandEnabler = rightHand.gameObject.AddComponent<EnableEventRelay>();
            rightHandEnabler.Enabled = new UnityEngine.Events.UnityEvent();
            rightHandEnabler.Disabled = new UnityEngine.Events.UnityEvent();
            rightHandEnabler.Enabled.AddListener(CmdRightHandEnable);
            rightHandEnabler.Disabled.AddListener(CmdRightHandDisable);

            for (int i = 0; i < enableGameObjectsIfLocal.Length; i++) enableGameObjectsIfLocal[i].SetActive(true);
        }


        leftHand.gameObject.SetActive(true);
        rightHand.gameObject.SetActive(true);
    }

    #region Hand Enabling/Disabling
    [Command]
    void CmdLeftHandEnable()
    {
        if (isClient) leftHand.gameObject.SetActive(true);
        else RpcLeftHandEnable();
    }

    [ClientRpc]
    void RpcLeftHandEnable()
    {
        leftHand.gameObject.SetActive(true);
    }

    [Command]
    void CmdLeftHandDisable()
    {
        if (isClient && isLocalPlayer) leftHand.gameObject.SetActive(false);
        else RpcLeftHandDisable();
    }

    [ClientRpc]
    void RpcLeftHandDisable()
    {
        leftHand.gameObject.SetActive(false);
        Debug.Log("Left Hand Disable");
    }

    // right hand
    [Command]
    void CmdRightHandEnable()
    {
        if (isClient && isLocalPlayer) rightHand.gameObject.SetActive(true);
        else RpcRightHandEnable();
    }

    [ClientRpc]
    void RpcRightHandEnable()
    {
        rightHand.gameObject.SetActive(true);
        Debug.Log("Rght Hand Enable");
    }

    [Command]
    void CmdRightHandDisable()
    {
        if (isClient && isLocalPlayer) rightHand.gameObject.SetActive(false);
        else RpcRightHandDisable();
    }

    [ClientRpc]
    void RpcRightHandDisable()
    {
        rightHand.gameObject.SetActive(false);
        Debug.Log("Right hand Disable");
    }
    #endregion
}
