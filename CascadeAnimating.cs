using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadeAnimating : MonoBehaviour
{
    public GameObject imagePrefab;
    private readonly GameObject[] images = new GameObject[25];

    private AudioController audioController;
    
    private float[] x = new float[25];
    private float[] y = new float[25];
    public float speed = 0.0001f;

    private void Awake()
    {
        audioController = GetComponent<AudioController>();
    }
    public void Animate()
    {
        StartCoroutine("Invoke");
        StartCoroutine("Del");
    }

    private IEnumerator Invoke()
    {
        audioController.sfxSource.clip = audioController.cards;
        audioController.sfxSource.Play();
        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.1f);
            x[i] = 8.5f; 
            y[i] = 8.2f;
            images[i] = Instantiate(imagePrefab, new Vector3(x[i], y[i], 10), Quaternion.identity) as GameObject;
            StartCoroutine("Movement1", i);
        }
    }
    
    private IEnumerator Del()
    {
        yield return new WaitForSeconds(3.5f);
        for (int i = 0; i < images.Length; i++)
        {
            Destroy(images[i]);
        }
    }

    private IEnumerator Movement1(int i)
    {
        while (images[i].transform.position.x != 0)
        {
            x[i] = x[i] - 0.5f;
            y[i] = y[i] - 0.5f;
            images[i].transform.position = new Vector3(x[i], y[i], 10);
            
            yield return new WaitForSeconds(0.01f);
        }
    }
}
