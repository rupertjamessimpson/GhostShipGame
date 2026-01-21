using System.Collections;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    private string monstersCurrentRoom;
    private bool knocking = false;
    private AudioSource audioSource;
    public AudioSource doorShutAudio;
    private new Renderer renderer;
    private Material windowMaterial;
    private Color darkRed = new Color(0.35f, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        windowMaterial = renderer.materials[1];

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMonsterRoom();

        if (PlayerPrefs.GetString("GameOver") != "true")
        {
            if (gameObject.name.Contains(monstersCurrentRoom))
            {
                WindowGlow();

                if (knocking == false)
                {
                    StartCoroutine(PlayAudioLoop());
                    knocking = true;
                }
            }
        }
        else
        {
            doorShutAudio.Stop();
        }
    }

    // Updates monsters location based on value stored in PlayerPref
    void UpdateMonsterRoom()
    {
        monstersCurrentRoom = PlayerPrefs.GetString("MonstersCurrentRoom");
    }

    // Gives the door window a red glowing effect
    void WindowGlow()
    {
        float step = Mathf.PingPong(Time.time, 1);
        windowMaterial.color = Color.Lerp(Color.black, darkRed, step);
    }

    // IEnum to play knocking loop if monster is in room
    private IEnumerator PlayAudioLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            audioSource.Play();
            yield return new WaitForSeconds(15f);
        }
    }
}
