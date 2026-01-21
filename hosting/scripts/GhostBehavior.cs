using System;
using System.Collections;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(Sing());
    }

    // Enum to trigger singing random event
    private IEnumerator Sing()
    {
        float delay = UnityEngine.Random.Range(0f, 1200f);
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }
}
