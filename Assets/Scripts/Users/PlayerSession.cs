using UnityEngine;

public static class PlayerSession
{
    public static string Username;

    public static void StartSession(string username)
    {
        Username = username;
    }
}