using UnityEngine;

public class ChairBehavior : MonoBehaviour, IPuzzleObject
{
    public bool leftSide = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetString("GluttonyPuzzle") == "solved")
        {
            gameObject.tag = "Untagged";
            
            Vector3 newPosition = transform.position;

            if (leftSide)
            {
                newPosition.x += 2f;
            }
            else
            {
                newPosition.x -= 2f;
            }

            transform.position = newPosition;
        }
    }

    // Trigger for puzzle behavior
    public void PuzzleBehavior()
    {
        Vector3 newPosition = transform.position;

        if (leftSide)
        {
            newPosition.x += 2f;
        }
        else
        {
            newPosition.x -= 2f;
        }

        transform.position = newPosition;

        GetComponent<AudioSource>().Play();
        gameObject.tag = "Untagged";
    }
}
