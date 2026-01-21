using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuBehavior : MonoBehaviour
{
    public bool creditsVisible = false;
    public GameObject credits;
    public GameObject buttons;
    public GameObject title;
    public GameObject creditHover;
    public GameObject creditNormal;
    public GameObject returnHover;
    public GameObject returnNormal;
    public GameObject startSoundSource;

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        buttons.gameObject.SetActive(false);
        title.gameObject.SetActive(false);

        AudioSource audio = startSoundSource.GetComponent<AudioSource>();
        audio.Play();

        StartCoroutine(LoadSceneAfterAudio(audio));
    }

    private IEnumerator LoadSceneAfterAudio(AudioSource audio)
    {
        yield return new WaitForSeconds(audio.clip.length);

        SceneManager.LoadScene("MainDeck");
    }
    public void ShowCredits()
    {
        if (creditsVisible == false)
        {
            creditsVisible = true;
            buttons.gameObject.SetActive(false);
            title.gameObject.SetActive(false);
            credits.gameObject.SetActive(true);

            returnHover.SetActive(false);
            returnNormal.SetActive(true);
        }
        else
        {
            creditsVisible = false;
            buttons.gameObject.SetActive(true);
            title.gameObject.SetActive(true);
            credits.gameObject.SetActive(false);

            creditHover.SetActive(false);
            creditNormal.SetActive(true);
        }
    }
}
