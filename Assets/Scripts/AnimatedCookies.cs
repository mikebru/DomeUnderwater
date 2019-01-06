using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCookies : MonoBehaviour {

    public enum AnimMode
    {
        forwards,
        backwards,
        random
    }

    public Texture2D[] textures;
    public float fps = 15;

    public AnimMode animMode = AnimMode.forwards;

    private int frameNr = 0;
    private Light cLight;
    private Projector projector;

    public bool useProjector = false;

    void Start()
    {
        cLight = GetComponent(typeof(Light)) as Light;
        projector = GetComponent<Projector>();

        if (cLight == null && projector == null)
        {
            Debug.LogWarning("AnimateCookieTexture: No light found on this gameObject", this);
            enabled = false;
        }

        StartCoroutine("switchCookie");
    }



    IEnumerator switchCookie()
    {
        while (true)
        {

            if (useProjector == false)
            {
                cLight.cookie = textures[frameNr];
            }
            else
            {
                projector.material.SetTexture("_ShadowTex", textures[frameNr]);
            }

            yield return new WaitForSeconds(1.0f / fps);
            switch (animMode)
            {
                case AnimMode.forwards: frameNr++; if (frameNr >= textures.Length) frameNr = 0; break;
                case AnimMode.backwards: frameNr--; if (frameNr < 0) frameNr = textures.Length - 1; break;
                case AnimMode.random: frameNr = Random.RandomRange(0, textures.Length); break;
            }
        }
    }

}
