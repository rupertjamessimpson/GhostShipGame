using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentTrigger : MonoBehaviour
{
    private bool isTriggered = false;

    [Header("Debug Settings")]
    public KeyCode testKey = KeyCode.Alpha1; // Use numbers 1-7 for testing

    void Update()
    {
        // Debug keyboard trigger
        if (!isTriggered && Input.GetKeyDown(testKey))
        {
            TriggerEquipment();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name} with tag: {other.gameObject.tag}");
        
        if (!isTriggered && other.CompareTag("Player"))
        {
            Debug.Log("Player recognized, triggering equipment");
            TriggerEquipment();
            
            // Destroy the equipment object after triggering
            Debug.Log("Destroying equipment object");
            Destroy(gameObject);
        }
    }

    private void TriggerEquipment()
    {
        isTriggered = true;
        
        // Try to get the SphereManager instance
        SphereManager sphereManager = SphereManager.Instance;
        
        if (sphereManager != null)
        {
            sphereManager.ActivateSphere(gameObject.tag);
            Debug.Log($"[EquipmentTrigger] Equipment '{gameObject.name}' triggered and sphere activated");
        }
        else
        {
            Debug.Log($"[EquipmentTrigger] Equipment '{gameObject.name}' triggered, but SphereManager.Instance is null");
            
            // Handle scene-specific logic
            HandleSceneSpecificLogic();
        }
    }
    
    private void HandleSceneSpecificLogic()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Library scene specific logic
        if (currentScene == "Library")
        {
            HandleLibrarySceneLogic();
        }
        // Add other scene-specific handlers here if needed
    }
    
    private void HandleLibrarySceneLogic()
    {
        // Store that we've collected the coin in PlayerPrefs
        PlayerPrefs.SetInt("CoinCollected", 1);
        PlayerPrefs.Save();
        
        Debug.Log("Coin collected in Library. This will be processed when returning to MainDeck.");
    }
}
