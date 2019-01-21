using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class SceneTimer : MonoBehaviour
{
    public bool AutoStart = false;

    public PlayableDirector cargoTimeline;

    public Transform TrashSpawnPoint;
    public GameObject trashGenerator;

    private GameObject trashGen;

    public float fadeTime = 10;
    public MeshRenderer DissolveDome;

    public AudioControl audioController;

    public UnityEvent CargoShipEvent;
    public UnityEvent StopVideoEvent;

    // Start is called before the first frame update
    void Start()
    {
        if (AutoStart == true)
        {
            StartCoroutine(FadeIn());
        }
    }

    public IEnumerator FadeIn()
    {

        audioController.PlayAudio(0);

        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;

            audioController.volumeSource(0, Mathf.Lerp(0, 1, t / fadeTime));

            DissolveDome.material.SetFloat("_Amount", Mathf.Lerp(0, 1, t / fadeTime));

            yield return new WaitForFixedUpdate();
        }


        StartCoroutine(CargoShipWait());
    }

    public IEnumerator CargoShipWait()
    {
        //wait for 2 minutes 
        yield return new WaitForSeconds(120);

        audioController.PlayAudio(1);

        CargoShipEvent.Invoke();
        cargoTimeline.Play();

        StartCoroutine(WaitStopVideo());

        StartCoroutine(DumpWait());
    }

    IEnumerator WaitStopVideo()
    {
        yield return new WaitForSeconds(27);

        StopVideoEvent.Invoke();
    }

    public IEnumerator DumpWait()
    {
        //wait for 20 seconds
        yield return new WaitForSeconds(20);

        audioController.StopAudio(1);
        audioController.PlayAudio(2);

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

            audioController.volumeSource(2, Mathf.Lerp(.5f, 0, t / fadeTime));


            DissolveDome.material.SetFloat("_Amount", Mathf.Lerp(1, 0, t / fadeTime));

            yield return new WaitForFixedUpdate();
        }

        audioController.StopAudio(2);

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





}
