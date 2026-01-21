using UnityEngine;
using TMPro;

public class MapBehavior : MonoBehaviour, IPuzzleObject
{
    public TMP_Text notice;
    public TMP_Text directions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetString("HasMap") == "true")
        {
            gameObject.SetActive(false);
        }
    }

    // Trigger for puzzle object behavior
    public void PuzzleBehavior()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();

        PlayerPrefs.SetString("HasMap", "true");
        PlayerPrefs.Save();

        notice.SetText("Map found");
        directions.SetText("Press Space to show map");
        notice.gameObject.SetActive(true);
        directions.gameObject.SetActive(true);

        gameObject.tag = "Untagged";
        GetComponent<MeshRenderer>().enabled = false;
    }
}
