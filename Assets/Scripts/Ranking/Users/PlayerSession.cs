public static class PlayerSession
{
    public static string Username { get; private set; }
    public static int Score { get; private set; }

    public static void StartSession(string username)
    {
        Username = username;
        Score = 0;
    }

    public static void SetScore(int score)
    {
        Score = score;
    }
}