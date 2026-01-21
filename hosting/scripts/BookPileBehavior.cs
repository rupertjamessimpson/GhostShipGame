using UnityEngine;

public class BookPileBehavior : MonoBehaviour, IPuzzleObject
{
    public float moveDistance = 1.8f;
    public string position;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (PlayerPrefs.GetString("EnvyPuzzle") == "solved")
        {
            SetSolvedPosition();
        }
    }

    // Trigger for puzzle interaction 
    public void PuzzleBehavior()
    {
        if (position == "up")
        {
            transform.position -= new Vector3(0, moveDistance, 0);
            audioSource.Play();
            position = "down";
        }
        else
        {
            transform.position += new Vector3(0, moveDistance, 0);
            audioSource.Play();
            position = "up";
        }
    }

    // Set to stay after solving
    public void SetSolvedPosition()
    {
        string solvedPosition = PlayerPrefs.GetString("EnvySolvePosition");
        if (position != solvedPosition)
        {
            if (solvedPosition == "up")
            {
                transform.position += new Vector3(0, moveDistance, 0);
            }
            else
            {
                transform.position -= new Vector3(0, moveDistance, 0);
            }
        }
    }
}
