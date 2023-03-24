using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public static bool openInfo;

    public AudioClip SEok;
    public Text PressSpaceKey;
    float sin = 0;
    public GameObject finishinfo;

    public GameObject[] BG;
    Renderer BG1, BG2;

    // Start is called before the first frame update
    void Start()
    {
        openInfo = false;
        BG1 = GetComponent<Renderer>();

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        PressSpaceKey.color = new Color(1, 1, 1, sin);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            finishinfo.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        sin = getSinCurve() + 0.8f;
    }

    float getSinCurve()
    {
        float T = 0.5f;
        float f = 1.0f / T;
        float sin = Mathf.Sin(2 * Mathf.PI * f * Time.time);

        return sin;
    }
}
