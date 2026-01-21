using UnityEngine;

public class LustPuzzleManager : MonoBehaviour
{
    public enum LustPuzzleState
    {
        Idle,
        PlayerNearby,
        PuzzleSolved
    }
    public static LustPuzzleManager Instance;
    public GameObject targetBed;
    public Transform player;
    public float muteDistance = 4f;
    private AudioSource bedAudio;
    private LustPuzzleState currentState = LustPuzzleState.Idle;

    // Plays before Start
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initializes puzzle state and sets up audio reference
    void Start()
    {
        if (targetBed != null)
        {
            bedAudio = targetBed.GetComponent<AudioSource>();

            if (PlayerPrefs.GetString("LustPuzzle") == "solved")
            {
                currentState = LustPuzzleState.PuzzleSolved;

                if (bedAudio != null)
                {
                    bedAudio.Stop();
                }
            }
        }
        else
        {
            Debug.LogWarning("Target bed not assigned in LustPuzzleBehavior.");
        }
    }

    // Updates the FSM state each frame based on player distance
    void Update()
    {
        if (currentState == LustPuzzleState.PuzzleSolved || targetBed == null || player == null) return;

        float distance = Vector3.Distance(player.position, targetBed.transform.position);

        switch (currentState)
        {
            case LustPuzzleState.Idle:
                if (distance < muteDistance)
                {
                    SetState(LustPuzzleState.PlayerNearby);
                }
                break;

            case LustPuzzleState.PlayerNearby:
                if (distance >= muteDistance)
                {
                    SetState(LustPuzzleState.Idle);
                }
                break;
        }
    }

    // Sets the FSM state and applies corresponding behavior
    private void SetState(LustPuzzleState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case LustPuzzleState.Idle:
                if (bedAudio != null && !bedAudio.isPlaying)
                    bedAudio.Play();

                if (bedAudio != null)
                    bedAudio.volume = 0.01f;
                break;

            case LustPuzzleState.PlayerNearby:
                if (bedAudio != null)
                    bedAudio.volume = 0f;
                break;

            case LustPuzzleState.PuzzleSolved:
                if (bedAudio != null)
                    bedAudio.Stop();
                break;
        }
    }

    // Called when the correct bed is solved
    public void PuzzleSolved()
    {
        PlayerPrefs.SetString("LustPuzzle", "solved");
        SetState(LustPuzzleState.PuzzleSolved);
        UntagAllBeds();
    }

    // Saves state of covers
    public void SaveBedState(string bedID, bool coversAreDown)
    {
        PlayerPrefs.SetInt(bedID, coversAreDown ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Gets the state of bed covers
    public bool GetBedState(string bedID)
    {
        return PlayerPrefs.GetInt(bedID, 0) == 1;
    }

    // Untags all beds in the scene
    public void UntagAllBeds()
    {
        GameObject[] beds = GameObject.FindGameObjectsWithTag("PuzzleObject");

        foreach (GameObject bed in beds)
        {
            bed.tag = "Untagged";
        }
    }
}
