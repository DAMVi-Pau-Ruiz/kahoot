using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public static class LeaderboardManager
{
    private static string leaderboardFolder = Application.persistentDataPath + "/leaderboards";

    public static void SaveScore(string kahootPath, string username, int score, string image = "")
    {
        if (!Directory.Exists(leaderboardFolder))
        {
            Directory.CreateDirectory(leaderboardFolder);
        }

        string kahootName = Path.GetFileNameWithoutExtension(kahootPath);

        string xmlPath = Path.Combine(leaderboardFolder, kahootName + "_leaderboard.xml");

        XmlDocument document = new XmlDocument();

        if (File.Exists(xmlPath))
        {
            document.Load(xmlPath);
        }
        else
        {
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            document.AppendChild(declaration);

            XmlElement root = document.CreateElement("Leaderboard");
            document.AppendChild(root);
        }

        XmlElement player = document.CreateElement("Player");
        
        XmlElement imageElement = document.CreateElement("Image");
        imageElement.InnerText = image;

        XmlElement usernameElement = document.CreateElement("Username");
        usernameElement.InnerText = username;

        XmlElement scoreElement = document.CreateElement("Score");
        scoreElement.InnerText = score.ToString();

        player.AppendChild(imageElement);
        player.AppendChild(usernameElement);
        player.AppendChild(scoreElement);

        document.DocumentElement.AppendChild(player);
        
        document.Save(xmlPath);

        Debug.Log("Leaderboard guardado en: " + xmlPath);
    }

    public static List<LeaderboardPlayer> LoadLeaderboard(string kahootPath)
    {
        List<LeaderboardPlayer> players = new List<LeaderboardPlayer>();

        string kahootName = Path.GetFileNameWithoutExtension(kahootPath);
        string xmlPath = Path.Combine(leaderboardFolder, kahootName + "_leaderboard.xml");

        if (!File.Exists(xmlPath))
        {
            return players;
        }

        XmlDocument document = new XmlDocument();
        document.Load(xmlPath);

        XmlNodeList playerNodes = document.SelectNodes("/Leaderboard/Player");

        foreach (XmlNode node in playerNodes)
        {
            LeaderboardPlayer player = new LeaderboardPlayer();

            player.image = node["Image"].InnerText;
            player.username = node["Username"].InnerText;
            int.TryParse(node["Score"].InnerText, out player.score);

            players.Add(player);
        }

        players.Sort((a, b) => b.score.CompareTo(a.score));

        return players;
    }

    public static void CreateLeaderboardIfNotExists(string kahootPath)
    {
        if (!Directory.Exists(leaderboardFolder))
        {
            Directory.CreateDirectory(leaderboardFolder);
        }

        string kahootName = Path.GetFileNameWithoutExtension(kahootPath);
        string xmlPath = Path.Combine(leaderboardFolder, kahootName + "_leaderboard.xml");

        if (File.Exists(xmlPath))
        {
            return;
        }

        XmlDocument document = new XmlDocument();

        XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
        document.AppendChild(declaration);

        XmlElement root = document.CreateElement("Leaderboard");
        document.AppendChild(root);

        document.Save(xmlPath);

        Debug.Log("Leaderboard creado: " + xmlPath);
    }
}
