using UnityEngine;

public class CrashBehavior : MonoBehaviour
{
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("CrashOccurred", 0) == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            anim.Play("CrashAnimation");
            Invoke("OnAnimationComplete", 3f);

            PlayerPrefs.SetInt("CrashOccurred", 1);
            PlayerPrefs.Save();
        }
    }

    // Removes game object once animation completes
    void OnAnimationComplete()
    {
        gameObject.SetActive(false);
    }
}
