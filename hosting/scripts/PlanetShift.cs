using UnityEngine;

public class PlanetShift : MonoBehaviour
{
    public bool moveForward = true;
    public bool moveOut = false;

    void Start()
    {
        string objectName = gameObject.name;
        string puzzleKey = objectName.Replace("Planet", "") + "Puzzle";

        if (PlayerPrefs.GetString(puzzleKey) != "solved")
        {
            Vector3 newPosition = transform.position;

            if (moveForward)
            {
                newPosition.z += 50;
            }
            else
            {
                newPosition.z -= 50;
            }

            if (moveOut)
            {
                newPosition.x += 200;
            }

            transform.position = newPosition;
        }
    }
}
