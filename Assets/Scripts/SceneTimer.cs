using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SceneTimer : MonoBehaviour
{
    public bool AutoStart = false;

    public PlayableDirector cargoTimeline;

    public Transform TrashSpawnPoint;
    public GameObject trashGenerator;

    private GameObject trashGen;

    public float fadeTime = 10;
    public MeshRenderer DissolveDome;

    // Start is called before the first frame update
    void Start()
    {
        if (AutoStart == true)
        {
            StartCoroutine(FadeIn());
        }
    }

    public IEnumerator CargoShipWait()
    {
        //wait for 3 minutes 
        yield return new WaitForSeconds(10);


        cargoTimeline.Play();

        StartCoroutine(DumpWait());
    }

    public IEnumerator DumpWait()
    {
        //wait for 30 seconds
        yield return new WaitForSeconds(20);

        trashGen = Instantiate(trashGenerator, TrashSpawnPoint.transform);

        trashGen.transform.localPosition = Vector3.zero;

        //wait for 2.5 minutes
        yield return new WaitForSeconds(150);

        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {

        float t = 0;

        while(t < fadeTime)
        {
            t += Time.deltaTime;

            DissolveDome.material.SetFloat("_Amount", Mathf.Lerp(1, 0, t / fadeTime));

            yield return new WaitForFixedUpdate();
        }


        yield return new WaitForSeconds(1);


        Destroy(trashGen);

        //toggle cargoship
        cargoTimeline.time = 0;
        cargoTimeline.Play();

        yield return new WaitForSeconds(.1f);

        cargoTimeline.Stop();

        yield return new WaitForSeconds(5);

        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {

        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;

            DissolveDome.material.SetFloat("_Amount", Mathf.Lerp(0, 1, t / fadeTime));

            yield return new WaitForFixedUpdate();
        }


        StartCoroutine(CargoShipWait());

    }



}
