using UnityEngine;

public class CoinBehavior : MonoBehaviour, IPuzzleObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetString("GreedPuzzle") == "solved")
        {
            gameObject.SetActive(false);
        }
    }

    // Trigger for puzzle object behavior
    public void PuzzleBehavior()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();

        gameObject.tag = "Untagged";
        GetComponent<MeshRenderer>().enabled = false;
    }
}
