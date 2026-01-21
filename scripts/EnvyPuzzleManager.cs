using UnityEngine;
using TMPro;

public class EnvyPuzzleManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    private bool puzzleSolved = false;
    public TMP_Text solvedText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObjects.Length == 0)
        {
            Debug.LogWarning("Book piles not set");
        }

        if (PlayerPrefs.GetString("EnvyPuzzle") == "solved")
        {
            puzzleSolved = true;
            PuzzleSolved();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleSolved) return;

        if (gameObjects.Length == 0) return;

        BookPileBehavior firstPile = gameObjects[0].GetComponent<BookPileBehavior>();
        if (firstPile == null) return;

        string targetPosition = firstPile.position;
        bool allSame = true;

        foreach (GameObject obj in gameObjects)
        {
            BookPileBehavior pile = obj.GetComponent<BookPileBehavior>();
            if (pile == null || pile.position != targetPosition)
            {
                allSame = false;
                break;
            }
        }

        if (allSame)
        {
            puzzleSolved = true;
            PlayerPrefs.SetString("EnvySolvePosition", targetPosition);
            if (solvedText)
            {
                solvedText.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Puzzle completion UI not set");
            }
            PuzzleSolved();
        }
    }

    // Saves puzzle solution and makes objects un-interactable
    void PuzzleSolved()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.tag = "Untagged";
        }

        PlayerPrefs.SetString("EnvyPuzzle", "solved");
        PlayerPrefs.Save();
    }
}
