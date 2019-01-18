using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator WaitDestory(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Destroy(this.gameObject);

    }

}
