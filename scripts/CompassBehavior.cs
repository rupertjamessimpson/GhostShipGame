using UnityEngine;
using TMPro;

public class CompassBehavior : MonoBehaviour, IPuzzleObject
{
    public TMP_Text notice;
    public TMP_Text directions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetString("HasCompass") == "true")
        {
            gameObject.SetActive(false);
        }
    }

    // Trigger for puzzle object behavior
    public void PuzzleBehavior()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();

        PlayerPrefs.SetString("HasCompass", "true");
        PlayerPrefs.Save();

        notice.SetText("Compass found");
        if (PlayerPrefs.GetString("HasMap") == "true")
        {
            directions.SetText("Puzzle locations displayed on map");
        }
        else
        {
            directions.SetText("Find map to show puzzle locations");
        }

        notice.gameObject.SetActive(true);
        directions.gameObject.SetActive(true);

        gameObject.tag = "Untagged";
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
