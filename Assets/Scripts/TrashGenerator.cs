using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    public float spawnRate = .5f;
    public GameObject[] trashPrefabs;
    public int Scale = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(trashSpawn());
    }


    IEnumerator trashSpawn()
    {
        int random = Random.Range(0, trashPrefabs.Length);
     
        Vector3 randomPosition = new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));

        GameObject newTrash = Instantiate(trashPrefabs[random], this.transform);

        newTrash.transform.localPosition = randomPosition;
        newTrash.transform.Rotate(randomPosition * 10);
        newTrash.transform.localScale = new Vector3(Scale, Scale, Scale);

        StartCoroutine(newTrash.GetComponent<DestroyTime>().WaitDestory(spawnRate / .001f));

        yield return new WaitForSeconds(spawnRate);

        StartCoroutine(trashSpawn());

    }

}
