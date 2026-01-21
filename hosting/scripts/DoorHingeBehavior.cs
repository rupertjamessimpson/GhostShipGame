using UnityEngine;

public class DoorHingeBehavior : MonoBehaviour
{
    private Animator animator;
    private string openTrigger = "Open";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Open()
    {
        if (animator != null)
            animator.SetTrigger(openTrigger);
    }
}