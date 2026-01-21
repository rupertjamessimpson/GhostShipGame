using UnityEngine;
using TMPro;

public class GreedPuzzleManager : MonoBehaviour
{
    public TMP_Text puzzleNotice;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("GreedPuzzle") == "solved")
        {
            return;
        }

        CheckForPuzzleSolved();
    }

    // Method to check for if all chairs are pushed in
    void CheckForPuzzleSolved()
    {
        GameObject[] chairs = GameObject.FindGameObjectsWithTag("PuzzleObject");

        if (chairs.Length == 0)
        {
            puzzleNotice.gameObject.SetActive(true);
            
            PlayerPrefs.SetString("GreedPuzzle", "solved");
            PlayerPrefs.Save();
        }
    }
}
