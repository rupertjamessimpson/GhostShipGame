using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Controller Settings")]
    Vector3 input;
    CharacterController controller;
    public float moveSpeed = 5f;
    private string door;

    [Header("UI Settings")]
    public TMP_Text location;
    public Image defaultReticle;
    public Image interactableReticle;
    public TMP_Text doorNameUI;
    public bool pauseMenuVisible = false;
    public GameObject pauseMenu;
    public GameObject resumeHover;
    public GameObject resumeNormal;
    public GameObject redPanel;
    public GameObject blackPanel;
    public GameObject blackPanelDoor;
    public GameObject gameEndedMenu;
    public GameObject gameEndedMenuText;
    public CanvasGroup gameEndedButtons;

    [Header("Map Settings")]
    public bool mapVisible;
    public GameObject map;
    public Image envy;
    public Image gluttony;
    public Image greed;
    public Image lust;
    public Image pride;
    public Image sloth;
    public Image wrath;

    [Header("Audio Settings")]
    private AudioSource audioSource;
    public AudioSource footstepSource;
    public AudioSource doorOpeningSource;
    public AudioSource backgroundAudio;
    public AudioSource monsterAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetString("GameWon") != "true" &&
            PlayerPrefs.GetString("GameOver") != "true")
        {
            StartCoroutine(PlayAudioLoop());
        }

        string spawnPoint = PlayerPrefs.GetString("SpawnPoint", "");

        if (PlayerPrefs.GetString("GameWon") == "true")
        {
            if (gameEndedMenu)
            {
                StartCoroutine(FadeInGameMenu());
            }

            spawnPoint = "SpawnPointGameWon";
            
            if (location)
            {
                location.gameObject.SetActive(false);
            }
        }

        if (!string.IsNullOrEmpty(spawnPoint))
        {
            GameObject spawnObject = GameObject.Find(spawnPoint);

            if (spawnObject != null)
            {
                transform.position = spawnObject.transform.position;
                Vector3 spawnRotation = spawnObject.transform.eulerAngles;
                transform.eulerAngles = new Vector3(spawnRotation.x, spawnRotation.y, 0);

                AudioSource doorAduioSource = spawnObject.gameObject.GetComponent<AudioSource>();
                if (doorAduioSource)
                {
                    doorAduioSource.Play();
                }
            }
        }

        PlayerPrefs.SetString("IsFirstScene", "No");
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenuVisible)
        {
            if (PlayerPrefs.GetString("GameWon") != "true" &&
                PlayerPrefs.GetString("GameOver") != "true")
            {
                if (PlayerPrefs.GetString("DoorOpening") != "true")
                {
                    HandleMovement();
                    HandleInteractions();
                    ShowMap();
                }
                DisplayUI();
                UpdateMap();
            }
        }

        if (PlayerPrefs.GetString("GameWon") != "true")
        {
            ListenForGameWon();
            ListenForPause();
        }

        if (PlayerPrefs.GetString("GameOver") == "true")
        {
            HandleGameOver();
        }
    }

    // Handle player movement
    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = transform.right * moveHorizontal + transform.forward * moveVertical;
        input.Normalize();

        controller.Move(input * moveSpeed * Time.deltaTime);

        bool isWalking = input.magnitude > 0.1f;

        if (isWalking)
        {
            if (footstepSource != null && !footstepSource.isPlaying)
            {
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource != null && footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
    }

    // Method to display the UI on the door
    void DisplayUI()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 4))
        {
            if (hit.collider.gameObject.tag != "Untagged" && hit.collider.gameObject.tag != "Player")
            {
                interactableReticle.gameObject.SetActive(true);
                defaultReticle.gameObject.SetActive(false);

                if (hit.collider.gameObject.name.Contains("Door") || hit.collider.gameObject.name.Contains("Stairs"))
                {
                    doorNameUI.text = FormatName(hit.collider.gameObject.tag);
                    doorNameUI.gameObject.SetActive(true);
                }
                else
                {
                    doorNameUI.gameObject.SetActive(false);
                }
            }
            else
            {
                doorNameUI.gameObject.SetActive(false);
                interactableReticle.gameObject.SetActive(false);
                defaultReticle.gameObject.SetActive(true);
            }
        }
        else
        {
            doorNameUI.gameObject.SetActive(false);
            interactableReticle.gameObject.SetActive(false);
            defaultReticle.gameObject.SetActive(true);
        }
    }

    // Method to add a space within the tag
    string FormatName(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        System.Text.StringBuilder newText = new System.Text.StringBuilder();
        newText.Append(input[0]);

        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]) && char.IsLower(input[i - 1]))
            {
                newText.Append(' ');
            }
            newText.Append(input[i]);
        }

        return newText.ToString();
    }

    // Scene loading logic for player opening door
    void HandleInteractions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 4))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.name.Contains("Door") || hitObject.name.Contains("Stairs"))
                {
                    string sceneName = hitObject.tag;
                    door = hitObject.name;

                    DoorHingeBehavior hinge = hitObject.GetComponentInParent<DoorHingeBehavior>();
                    if (hinge != null)
                    {
                        hinge.Open();
                    }

                    HandleLoadDoorScene(sceneName);
                }

                if (hitObject.tag == "PuzzleObject")
                {
                    InteractWithPuzzle(hitObject);
                }
            }
        }
    }

    // Scene loading logic
    void HandleLoadDoorScene(string sceneName)
    {
        string spawnPoint = "SpawnPoint" + SceneManager.GetActiveScene().name;

        if (SceneExists(sceneName))
        {
            OpenDoor(sceneName, spawnPoint);
        }
        else
        {
            Debug.LogWarning("Could not find scene " + sceneName);
        }
    }

    // Checks if scene exists
    bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if (scenePath.Contains(sceneName))
                return true;
        }
        return false;
    }

    // Loads new scene
    void OpenDoor(string sceneName, string spawnPoint)
    {
        PlayerPrefs.SetString("SpawnPoint", spawnPoint);
        PlayerPrefs.Save();

        footstepSource.Stop();

        PlayerPrefs.SetString("DoorOpening", "true");
        StartCoroutine(DoorOpenDelay(sceneName));
    }

    IEnumerator DoorOpenDelay(string sceneName)
    {
        if (!door.Contains("Stairs"))
        {
            doorOpeningSource.Play();
        }
        blackPanelDoor.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    // Handle puzzle object interaction
    void InteractWithPuzzle(GameObject puzzleObject)
    {
        IPuzzleObject puzzle = puzzleObject.GetComponent<IPuzzleObject>();
        if (puzzle != null)
        {
            puzzle.PuzzleBehavior();
        }
        else
        {
            Debug.LogWarning("Clicked object has no IPuzzleObject component attached.");
        }
    }

    // IEnum to play breathing loop
    private IEnumerator PlayAudioLoop()
    {
        while (PlayerPrefs.GetString("GameOver") != "true")
        {
            yield return new WaitForSeconds(2f);
            if (audioSource && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            yield return new WaitForSeconds(2f);
        }
    }

    // Listener in Update() to check for pause button
    void ListenForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    // Behavior to toggle pause menu
    public void Pause()
    {
        if (pauseMenu == null)
        {
            Debug.LogWarning("PauseMenu not found in scene!");
            return;
        }

        pauseMenuVisible = !pauseMenuVisible;
        pauseMenu.SetActive(pauseMenuVisible);
        
        resumeHover.SetActive(false);
        resumeNormal.SetActive(true);

        Time.timeScale = pauseMenuVisible ? 0f : 1f;

        Cursor.visible = pauseMenuVisible;
        Cursor.lockState = pauseMenuVisible ? CursorLockMode.None : CursorLockMode.Locked;

        if (backgroundAudio != null)
        {
            if (pauseMenuVisible)
            {
                backgroundAudio.Pause();
            }
            else
            {
                backgroundAudio.UnPause();
            }
        }
        if (monsterAudio != null)
        {
            if (pauseMenuVisible)
            {
                monsterAudio.Pause();
            }
            else
            {
                monsterAudio.UnPause();
            }
        }
    }

    // Function to restart game from menu and clear PlayerPrefs
    public void Restart()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("StartMenu");
    }

    // Function to restart game from main deck and clear PlayerPrefs
    public void RestartAfterWin()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("MainDeck");
    }

    // Function to restart game from main deck and clear PlayerPrefs
    public void RestartAfterLose()
    {
        PlayerPrefs.SetString("GameOver", "");
        PlayerPrefs.SetString("SpawnPoint", "");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainDeck");
    }

    // Function to update the images in the map compass
    void UpdateMap()
    {
        if (PlayerPrefs.GetString("HasCompass") != "true")
        {
            envy.gameObject.SetActive(false);
            gluttony.gameObject.SetActive(false);
            greed.gameObject.SetActive(false);
            lust.gameObject.SetActive(false);
            pride.gameObject.SetActive(false);
            sloth.gameObject.SetActive(false);
            wrath.gameObject.SetActive(false);

            return;
        }
        else
        {
            envy.gameObject.SetActive(true);
            gluttony.gameObject.SetActive(true);
            greed.gameObject.SetActive(true);
            lust.gameObject.SetActive(true);
            pride.gameObject.SetActive(true);
            sloth.gameObject.SetActive(true);
            wrath.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetString("EnvyPuzzle") == "solved")
        {
            envy.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetString("GluttonyPuzzle") == "solved")
        {
            gluttony.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetString("GreedPuzzle") == "solved")
        {
            greed.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetString("LustPuzzle") == "solved")
        {
            lust.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetString("PridePuzzle") == "solved")
        {
            pride.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetString("SlothPuzzle") == "solved")
        {
            sloth.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetString("WrathPuzzle") == "solved")
        {
            wrath.gameObject.SetActive(false);
        }
    }

    // Toggle to show map
    void ShowMap()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerPrefs.GetString("HasMap") == "true")
            {
                if (map)
                {
                    if (mapVisible)
                    {
                        mapVisible = false;
                        map.gameObject.SetActive(false);
                    }
                    else
                    {
                        mapVisible = true;
                        map.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogWarning("Map not found");
                }
            }
        }
    }

    // Method to continously check for game won state
    void ListenForGameWon()
    {
        if (PlayerPrefs.GetString("EnvyPuzzle") == "solved" &&
            PlayerPrefs.GetString("GluttonyPuzzle") == "solved" &&
            PlayerPrefs.GetString("GreedPuzzle") == "solved" &&
            PlayerPrefs.GetString("LustPuzzle") == "solved" &&
            PlayerPrefs.GetString("PridePuzzle") == "solved" &&
            PlayerPrefs.GetString("SlothPuzzle") == "solved" &&
            PlayerPrefs.GetString("WrathPuzzle") == "solved")
        {
            PlayerPrefs.SetString("GameWon", "true");
            PlayerPrefs.Save();

            GameWon();
        }
    }

    // Method to trigger game won action
    void GameWon()
    {
        StartCoroutine(FadeScreen(blackPanel));
    }

    // Enum to fade in screen panel
    IEnumerator FadeScreen(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainDeck");
    }

    // Enum to fade in game win or over menus
    IEnumerator FadeInGameMenu()
    {
        yield return new WaitForSeconds(2f);

        if (gameEndedMenuText)
        {
            gameEndedMenuText.SetActive(true);
            yield return new WaitForSeconds(2f);
        }

        gameEndedMenu.SetActive(true);

        yield return new WaitForSeconds(3f);
        gameEndedButtons.alpha = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Trigger for game over animation and menu logic
    void HandleGameOver()
    {
        Animator camAnim = Camera.main.GetComponent<Animator>();
        camAnim.enabled = true;

        if (backgroundAudio)
        {
            backgroundAudio.Stop();
        }

        StartCoroutine(ShowRedAfterDelay());
    }

    // Enum to delay red screen for death animation
    IEnumerator ShowRedAfterDelay()
    {
        yield return new WaitForSeconds(6.7f);
        redPanel.SetActive(true);
        StartCoroutine(FadeInGameMenu());
    }
}
