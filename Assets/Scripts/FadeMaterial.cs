using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMaterial : MonoBehaviour
{
    public Material material;
    public string parameter;
    private Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        startColor = material.GetColor(parameter);
    }

    private void OnDisable()
    {
        material.SetColor(parameter, startColor);
    }

    public void resetMat()
    {
        material.SetColor(parameter, startColor);
    }

    public void FadeoutMat(float fadeTime)
    {
        StartCoroutine(FadeOut(fadeTime));

    }

    IEnumerator FadeOut(float fadeTime)
    {
        float t = 0;


        while(t < fadeTime)
        {
            t += Time.deltaTime;

            Color newColor = Color.Lerp(startColor, Color.clear, t / fadeTime);

            material.SetColor(parameter, newColor);

            yield return new WaitForFixedUpdate();
        }


    }


}
