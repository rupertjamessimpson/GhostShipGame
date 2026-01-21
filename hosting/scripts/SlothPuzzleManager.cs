using UnityEngine;

public class SlothPuzzleManager : MonoBehaviour
{
    public GameObject lightSwitch;
    public GameObject darkness;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetString("SlothPuzzle") == "solved")
        {
            lightSwitch.gameObject.tag = "Untagged";
            darkness.SetActive(false);
        }
    }
}
