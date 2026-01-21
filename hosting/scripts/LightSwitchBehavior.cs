using UnityEngine;
using TMPro;

public class LightSwitchBehavior : MonoBehaviour, IPuzzleObject
{
    public GameObject darkness;
    private AudioSource audioSource;
    public TMP_Text solvedText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Trigger for puzzle object interaction
    public void PuzzleBehavior()
    {
        audioSource.Play();
        darkness.SetActive(false);
        gameObject.tag = "Untagged";

        solvedText.gameObject.SetActive(true);

        PlayerPrefs.SetString("SlothPuzzle", "solved");
        PlayerPrefs.Save();
    }
}
