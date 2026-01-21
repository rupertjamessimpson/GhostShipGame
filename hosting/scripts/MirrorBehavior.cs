using UnityEngine;

public class MirrorBehavior : MonoBehaviour, IPuzzleObject
{
    public bool shattered = false;
    public Material CrackedMirrorMaterial;
    private Renderer mirrorRenderer;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mirrorRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetString("PridePuzzle") == "solved")
        {
            ShatterMirror();
        }
    }

    // IPuzzleObject logic for solving puzzle
    public void PuzzleBehavior()
    {
        audioSource.Play();
        ShatterMirror();
    }

    // Updates material
    public void ShatterMirror()
    {
        mirrorRenderer.material = CrackedMirrorMaterial;
        shattered = true;
        gameObject.tag = "Untagged";
    }
}
