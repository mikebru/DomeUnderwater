using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SceneTimer : MonoBehaviour
{
    public PlayableDirector cargoTimeline;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator CargoShipWait()
    {
        //wait for 3 minutes 
        yield return new WaitForSeconds(180);


        cargoTimeline.Play();
    }





}
