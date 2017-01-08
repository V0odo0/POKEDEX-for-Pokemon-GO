using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    public PokeData PokeData;
    public UserData UserData;
    public UserEvents UserEvents;

    [Space]
    public string AppName = "POKEDEX+ for Pokemon Go";
    public string AppVer = "v1.0";

    public string IssueReportUrl = "https://goo.gl/forms/MjeHq40wAk8PdSU33";
    public string DevTwitterLink = "https://twitter.com/Voodoo2211";

    void Awake()
    {
        Instance = this;

        Input.multiTouchEnabled = false;

        UserData = new UserData();
        UserData.LoadAll();
        UserEvents = new UserEvents();

        Debug.Log("Application initialized");
    }

    public void OnApplicationQuit()
    {
        SendUserEvents();

        UIManager.Instance.UpdateUserData();
        UserData.SaveAll();

        Debug.Log("Quit");
    }

    void SendUserEvents()
    {
        Analytics.CustomEvent("End of session", new Dictionary<string, object>
        {
            {"RealtimeSinceStartup", Time.realtimeSinceStartup},

            {"SearchUsed", UserEvents.SearchCount},
            {"SortUsed", UserEvents.SortCount},
            {"FavoritesUsed", UserEvents.FavoritesCount},
            {"PokemonPageOpened", UserEvents.PokemonPageCount},
            {"PokemonMoveOpened", UserEvents.PokemonMoveCount},
        });
    }

    public static void RegisterUserEvent(UserEventType eventType)
    {
        switch (eventType)
        {
            case UserEventType.SearchUsed:
                Instance.UserEvents.SearchCount++;
                break;
            case UserEventType.SortUsed:
                Instance.UserEvents.SortCount++;
                break;
            case UserEventType.FavoritesUsed:
                Instance.UserEvents.FavoritesCount++;
                break;
            case UserEventType.PokemonPageOpened:
                Instance.UserEvents.PokemonPageCount++;
                break;
            case UserEventType.PokemonMoveOpened:
                Instance.UserEvents.PokemonMoveCount++;
                break;
        }
    }
}

public delegate void YieldComplete();
public delegate void SwipeRight();
public delegate void SwipeLeft();
public delegate void ElementSelected(bool selected);

public struct UserEvents
{
    public int SearchCount;
    public int SortCount;
    public int FavoritesCount;
    public int PokemonPageCount;
    public int PokemonMoveCount;
}

public enum UserEventType
{
    SearchUsed, SortUsed, FavoritesUsed, PokemonPageOpened, PokemonMoveOpened
}
