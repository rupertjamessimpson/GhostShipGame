using UnityEngine;

public class ReturnBehavior : MonoBehaviour
{
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        // // REMOVE THIS
        // PlayerPrefs.SetString("GameWon", "true");

        if (PlayerPrefs.GetString("GameWon") != "true")
        {
            gameObject.SetActive(false);
            
            return;
        }

        anim.Play("ReturnAnimation");
        Invoke("OnAnimationComplete", 3f);
        PlayerPrefs.Save();
    }

    // Removes game object once animation completes
    void OnAnimationComplete()
    {
        gameObject.SetActive(false);
    }
}
