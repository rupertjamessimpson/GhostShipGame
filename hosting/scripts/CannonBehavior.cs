using UnityEngine;
using System.Collections;

public class CannonBehavior : MonoBehaviour, IPuzzleObject
{
    private AudioSource audioSource;
    public ParticleSystem particleEffect;
    public Transform cannonBallSpawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (PlayerPrefs.GetString("WrathPuzzle") == "solved")
        {
            gameObject.tag = "Untagged";
        }
    }

    // Coroutine to reset tag after delay
    private IEnumerator ResetTagAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (PlayerPrefs.GetString("WrathPuzzle") != "solved")
        {
            gameObject.tag = "PuzzleObject";
        }
    }

    // Trigger for puzzle object interaction
    public void PuzzleBehavior()
    {
        gameObject.tag = "Untagged";

        audioSource.Play();

        ParticleSystem effect = Instantiate(particleEffect, cannonBallSpawnPoint.position, cannonBallSpawnPoint.rotation);
        effect.Play();
        Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax + 0.5f);

        StartCoroutine(ResetTagAfterDelay(1f));

        WrathPuzzleManager.Instance.RegisterCannonFire(gameObject.name);
    }
}
