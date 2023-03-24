using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrool : MonoBehaviour
{
    float scroolspeed = 1f;
    Vector3 Top, Bot;

    // Start is called before the first frame update
    void Start()
    {
        Top = Camera.main.ScreenToWorldPoint(new Vector3(0, 1080f, 1.0f));
        Bot = Camera.main.ScreenToWorldPoint(new Vector3(0, -1080f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, Time.deltaTime * scroolspeed);
        if (transform.position.y <= Bot.y)
            transform.position = Top;
    }
}
