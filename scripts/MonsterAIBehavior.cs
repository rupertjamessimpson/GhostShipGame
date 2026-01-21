using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterAIBehavior : MonoBehaviour
{
    public AudioSource audioSource;
    private string monstersCurrentRoom;
    private Dictionary<string, List<string>> adjacentRooms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string playersCurrentRoom = SceneManager.GetActiveScene().name;
        string monstersPreviousRoom = PlayerPrefs.GetString("MonstersPreviousRoom");

        if (playersCurrentRoom == monstersPreviousRoom)
        {
            audioSource.Play();
            GameOver();
        }

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("MonstersNextRoom")))
        {
            monstersCurrentRoom = "CrewsQuarters";
        }
        else
        {
            monstersCurrentRoom = PlayerPrefs.GetString("MonstersNextRoom");
        }

        PlayerPrefs.SetString("MonstersCurrentRoom", monstersCurrentRoom);

        InitializeRoomConnections();
        MoveMonster();
    }

    // Triggers game over state
    void GameOver()
    {
        PlayerPrefs.SetString("GameOver", "true");
        PlayerPrefs.Save();
    }

    // Creates dictionary of adjacent rooms
    void InitializeRoomConnections()
    {
        adjacentRooms = new Dictionary<string, List<string>>()
        {
            { "CaptainsQuarters", new List<string> { "DiningHall", "MainHall" } },
            { "CrewsQuarters", new List<string> { "MainHall", "Washroom" } },
            { "MainHall", new List<string> { "CaptainsQuarters", "CrewsQuarters", "DiningHall", "Washroom" } },
            { "DiningHall", new List<string> { "CaptainsQuarters", "MainHall", "NavigationRoom", "Library", "Kitchen" } },
            { "NavigationRoom", new List<string> { "MainHall", "DiningHall" } },
            { "Library", new List<string> { "DiningHall", "NavigationRoom" } },
            { "Washroom", new List<string> { "CrewsQuarters", "MainHall" } },
            { "Kitchen", new List<string> { "DiningHall", "Storage" } },
            { "Storage", new List<string> { "Kitchen" } }
        };
    }

    // Moves the monster to a new room
    void MoveMonster()
    {
        List<string> possibleRooms = adjacentRooms[monstersCurrentRoom];

        string monstersNextRoom = possibleRooms[Random.Range(0, possibleRooms.Count)];

        PlayerPrefs.SetString("MonstersPreviousRoom", monstersCurrentRoom);
        PlayerPrefs.SetString("MonstersNextRoom", monstersNextRoom);
        PlayerPrefs.Save();
    }
}
