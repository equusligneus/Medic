using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenScript : MonoBehaviour
{
    [SerializeField]
    private float readingTime = 12f;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + readingTime)
            SceneManager.LoadScene("Level_01");
    }

    void LoadNextScene()
	{

	}
}
