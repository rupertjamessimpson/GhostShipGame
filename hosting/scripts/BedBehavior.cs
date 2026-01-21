using UnityEngine;
using TMPro;

public class BedBehavior : MonoBehaviour, IPuzzleObject
{
    public string bedID;
    public GameObject coversUp;
    public GameObject coversDown;
    private bool coversAreDown = false;
    public TMP_Text solvedUI;
    private AudioSource coversAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coversAudioSource = transform.Find("Covers").GetComponent<AudioSource>();
        coversAreDown = LustPuzzleManager.Instance.GetBedState(bedID);

        coversUp.SetActive(!coversAreDown);
        coversDown.SetActive(coversAreDown);

        if (PlayerPrefs.GetString("LustPuzzle")  == "solved")
        {
            gameObject.tag = "Untagged";

            if (bedID == "3")
            {
                Mute();
            }
        }

        if (coversAreDown)
        {
            gameObject.tag = "Untagged";
        }
    }

    // Mutes sfx
    public void Mute()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource)
        {
            audioSource.Stop();
        }
    }

    // Trigger for puzzle object interaction
    public void PuzzleBehavior()
    {
        if (!coversAreDown)
        {
            coversUp.SetActive(false);
            coversDown.SetActive(true);
            coversAreDown = true;
            coversAudioSource.Play();

            LustPuzzleManager.Instance.SaveBedState(bedID, true);

            if (bedID == "3")
            {
                if (solvedUI)
                {
                    solvedUI.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Solve UI not set");
                }

                LustPuzzleManager.Instance.PuzzleSolved();
            }

            gameObject.tag = "Untagged";
        }
    }
}
