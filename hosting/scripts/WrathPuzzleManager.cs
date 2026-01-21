using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WrathPuzzleManager : MonoBehaviour
{
    public static WrathPuzzleManager Instance;
    public TMP_Text solvedUI;
    private List<string> correctOrder = new() { "Cannon1", "Cannon2", "Cannon3" };
    private List<string> currentSequence = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("WrathPuzzle") == "solved")
        {
            UntagAllPuzzleObjects();
        }
    }

    // Called by CannonBehavior when a cannon is fired
    public void RegisterCannonFire(string cannonName)
    {
        currentSequence.Add(cannonName);

        // Keep the list from growing forever
        if (currentSequence.Count > correctOrder.Count)
        {
            currentSequence.RemoveAt(0);
        }

        if (IsCorrectSequence())
        {
            PuzzleSolved();
        }
    }

    // Checks if the latest entries match the correct sequence
    private bool IsCorrectSequence()
    {
        if (currentSequence.Count < correctOrder.Count)
        {
            return false;
        }

        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (currentSequence[i] != correctOrder[i])
            {
                return false;
            }
        }

        return true;
    }

    // Untags all puzzle objects in the scene
    public void UntagAllPuzzleObjects()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PuzzleObject");

        foreach (GameObject obj in objs)
        {
            obj.tag = "Untagged";
        }
    }

    // Handles puzzle success
    private void PuzzleSolved()
    {
        solvedUI.gameObject.SetActive(true);

        UntagAllPuzzleObjects();

        PlayerPrefs.SetString("WrathPuzzle", "solved");
    }
}
