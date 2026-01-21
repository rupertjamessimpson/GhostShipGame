using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 200f;
    [SerializeField] private float webGLMultiplier = 0.005f;

    private Transform playerBody;
    private float pitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetString("DoorOpening", "");
        playerBody = transform.parent.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            mouseSensitivity *= webGLMultiplier;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("GameWon") != "true" &&
            PlayerPrefs.GetString("GameOver") != "true" &&
            PlayerPrefs.GetString("DoorOpening") != "true")
        {

            float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            if (playerBody)
            {
                playerBody.Rotate(Vector3.up * moveX);
            }

            pitch -= moveY;
            pitch = Mathf.Clamp(pitch, -70, 70);
            transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }
}
