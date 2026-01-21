using UnityEngine;
using TMPro;

public class PridePuzzleManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    private bool puzzleSolved = false;
    public TMP_Text solvedText;
    bool allShattered = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObjects.Length == 0)
        {
            Debug.LogWarning("Book piles not set");
        }
        
        if (PlayerPrefs.GetString("PridePuzzle") == "solved")
        {
            puzzleSolved = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleSolved) return;

        allShattered = true;

        foreach (GameObject obj in gameObjects)
        {
            MirrorBehavior mirror = obj.GetComponent<MirrorBehavior>();
            if (mirror == null || !mirror.shattered)
            {
                allShattered = false;
                break;
            }
        }

        if (allShattered)
        {
            if (solvedText)
            {
                solvedText.gameObject.SetActive(true);
            }
            PuzzleSolved();
            puzzleSolved = true;
        }
    }

    // Saves puzzle solution
    void PuzzleSolved()
    {
        PlayerPrefs.SetString("PridePuzzle", "solved");
        PlayerPrefs.Save();
    }
}
