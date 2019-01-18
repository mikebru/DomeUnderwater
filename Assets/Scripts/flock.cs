using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour
{
    public float speed = 0.1f;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighborDistance = 3.0f;

    bool turning = false;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.localPosition, Vector3.zero) >= GlobalFlock.tankSize)
        {
            turning = true;
        }
        else
            turning = false;

        if(turning)
        {
            Vector3 direction = Vector3.zero - transform.localPosition;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  rotationSpeed * Time.deltaTime);
            speed = Random.RandomRange(0.5f, 1);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }     

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = GlobalFlock.allFish;

        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = GlobalFlock.goalPos;

        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.localPosition, this.transform.localPosition);
                if(dist <= neighborDistance)
                {
                    vcenter += go.transform.localPosition;
                    groupSize++;

                    if(dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.localPosition - go.transform.localPosition);
                    }

                    flock anotherFlock = go.GetComponent<flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPos - this.transform.localPosition);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.localPosition;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      rotationSpeed * Time.deltaTime);
        }
    }
}
