using UnityEngine;
using System.Collections.Generic;

public class SphereManager : MonoBehaviour
{
    // Created an instance to work between scenes
    public static SphereManager Instance;

    [System.Serializable]
    public class SphereEquipPair
    {
        public GameObject sphere;
        public string equipTag;
        public Vector3 finalPosition;
        public bool isActivated = false;
        public string pairName;
    }

    [Header("Cross Formation Settings")]
    public bool useAutomaticPositioning = true;
    public float crossSize = 10f;
    public float sphereHeight = 20f;
    public float moveSpeed = 5f;

    [Header("Sphere-Equipment Pairs")]
    public List<SphereEquipPair> spherePairs = new List<SphereEquipPair>();

    // Prevents duplicates
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (useAutomaticPositioning)
        {
            InitializeCrossPositions();
        }
        
        // Check if the coin was collected in the Library scene
        if (PlayerPrefs.HasKey("CoinCollected") && PlayerPrefs.GetInt("CoinCollected") == 1)
        {
            // Activate the coin sphere
            ActivateSphere("Coin");
            
            // Clear the flag so it doesn't activate again
            PlayerPrefs.DeleteKey("CoinCollected");
            PlayerPrefs.Save();
            
            Debug.Log("[SphereManager] Activated Coin sphere based on Library collection");
        }
        
        Debug.Log("[SphereManager] Initialized with " + spherePairs.Count + " sphere-equipment pairs");
    }

    private void InitializeCrossPositions()
    {
        if (spherePairs.Count != 7)
        {
            Debug.LogError("[SphereManager] Exactly 7 sphere-equipment pairs required!");
            return;
        }

        // Center sphere (Pride)
        spherePairs[0].finalPosition = new Vector3(0, sphereHeight, 0);

        // Vertical line of the cross (3 spheres)
        spherePairs[1].finalPosition = new Vector3(0, sphereHeight + crossSize, 0);     // Top
        spherePairs[2].finalPosition = new Vector3(0, sphereHeight - crossSize, 0);     // Bottom

        // Horizontal line of the cross (4 spheres)
        spherePairs[3].finalPosition = new Vector3(crossSize, sphereHeight, 0);         // Right
        spherePairs[4].finalPosition = new Vector3(-crossSize, sphereHeight, 0);        // Left
        spherePairs[5].finalPosition = new Vector3(crossSize/2, sphereHeight, 0);       // Mid-Right
        spherePairs[6].finalPosition = new Vector3(-crossSize/2, sphereHeight, 0);      // Mid-Left

        Debug.Log("[SphereManager] Cross positions initialized");
    }

    public void ActivateSphere(string equipTag)
    {
        foreach (var pair in spherePairs)
        {
            if (pair.equipTag == equipTag)
            {
                if (pair.isActivated)
                {
                    Debug.Log($"[SphereManager] Pair '{pair.pairName}' already activated!");
                    return;
                }

                pair.isActivated = true;
                if (pair.sphere != null)
                {
                    StartCoroutine(MoveSphereToPosition(pair));
                    Debug.Log($"[SphereManager] Activated pair '{pair.pairName}' with tag: {equipTag}");
                }
                else
                {
                    Debug.LogError($"[SphereManager] Sphere reference missing for pair '{pair.pairName}'!");
                }
                break;
            }
        }
    }

    private System.Collections.IEnumerator MoveSphereToPosition(SphereEquipPair pair)
    {
        Vector3 startPos = pair.sphere.transform.position;
        float journeyLength = Vector3.Distance(startPos, pair.finalPosition);
        float startTime = Time.time;
        float distanceCovered = 0f;

        while (distanceCovered < journeyLength)
        {
            distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            
            pair.sphere.transform.position = Vector3.Lerp(startPos, pair.finalPosition, fractionOfJourney);
            yield return null;
        }

        // Ensure final position is exact
        pair.sphere.transform.position = pair.finalPosition;
        Debug.Log($"[SphereManager] Sphere '{pair.pairName}' reached final position");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
