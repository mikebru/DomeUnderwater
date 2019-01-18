using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject goalPrefab;
    public static int tankSize = 5;

    public Vector3 TankVector;

    public int numFish = 10;
    public static GameObject[] allFish;

    public static Vector3 goalPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-TankVector.x, TankVector.x),
                                      Random.Range(-TankVector.y, TankVector.y),
                                      Random.Range(-TankVector.z, TankVector.z));


            allFish[i] = (GameObject) Instantiate(fishPrefab, this.transform);
            allFish[i].transform.localPosition = pos;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 10000) < 50) // increase the number of 0 in 10000 here if you want the fish to randomly move less frequently
                                        // and decrease if you want more frequenty
        {
            goalPos = new Vector3(Random.Range(-tankSize, tankSize),
                                  Random.Range(0, tankSize),
                                  Random.Range(-tankSize, tankSize));

            goalPrefab.transform.localPosition = goalPos;

            goalPos = goalPrefab.transform.localPosition;
        }
    }
}
