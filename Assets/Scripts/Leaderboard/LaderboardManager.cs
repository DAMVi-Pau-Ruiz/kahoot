
using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public static class LeaderboardManager
{
    private static string leaderboardFolder =
        Application.persistentDataPath + "/leaderboards";

    public static void SaveScore(string kahootPath, string username, int score, string image = "")
    {
        string kahootName = Path.GetFileNameWithoutExtension(kahootPath);
        string xmlPath = Path.Combine(leaderboardFolder, kahootName + "_leaderboard.xml");

        try
        {
            Directory.CreateDirectory(leaderboardFolder);

            XmlDocument document = new XmlDocument();

            if (File.Exists(xmlPath))
            {
                document.Load(xmlPath);

                if (document.DocumentElement == null ||
                    document.DocumentElement.Name != "Leaderboard")
                {
                    RegisterLeaderboardError(kahootName, "Leaderboard XML inválido", "El archivo no contiene una estructura Leaderboard válida.");

                    return;
                }
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
        catch (Exception e)
        {
            RegisterLeaderboardError(kahootName, "Error guardando leaderboard", "No se ha podido guardar el leaderboard. " + e.Message);
        }
    }

    public static List<LeaderboardPlayer> LoadLeaderboard(string kahootPath)
    {
        List<LeaderboardPlayer> players = new List<LeaderboardPlayer>();

        string kahootName = Path.GetFileNameWithoutExtension(kahootPath);
        string xmlPath = Path.Combine(leaderboardFolder, kahootName + "_leaderboard.xml");

        if (!File.Exists(xmlPath))
            return players;

        try
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlPath);

            if (document.DocumentElement == null ||
                document.DocumentElement.Name != "Leaderboard")
            {
                RegisterLeaderboardError(kahootName, "Leaderboard XML inválido", "El archivo no contiene una estructura Leaderboard válida.");

                return players;
            }

            XmlNodeList playerNodes = document.SelectNodes("/Leaderboard/Player");

            foreach (XmlNode node in playerNodes)
            {
                if (node["Image"] == null ||
                    node["Username"] == null ||
                    node["Score"] == null)
                {
                    RegisterLeaderboardError(kahootName, "Datos de jugador incompletos", "Un jugador del leaderboard no contiene todos los campos necesarios.");

                    continue;
                }

                if (!int.TryParse(node["Score"].InnerText, out int score))
                {
                    RegisterLeaderboardError(kahootName, "Puntuación inválida", "Se ha encontrado una puntuación que no tiene un formato numérico válido.");

                    continue;
                }

                LeaderboardPlayer player = new LeaderboardPlayer();

                player.image = node["Image"].InnerText;
                player.username = node["Username"].InnerText;
                player.score = score;

                players.Add(player);
            }

            players.Sort((a, b) => b.score.CompareTo(a.score));
        }
        catch (Exception e)
        {
            RegisterLeaderboardError(kahootName, "Error leyendo leaderboard", "No se ha podido leer el archivo XML. " + e.Message);
        }

        return players;
    }

    public static void CreateLeaderboardIfNotExists(string kahootPath)
    {
        string kahootName = Path.GetFileNameWithoutExtension(kahootPath);
        string xmlPath = Path.Combine(leaderboardFolder, kahootName + "_leaderboard.xml");

        try
        {
            Directory.CreateDirectory(leaderboardFolder);

            if (File.Exists(xmlPath))
                return;

            XmlDocument document = new XmlDocument();

            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            document.AppendChild(declaration);

            XmlElement root = document.CreateElement("Leaderboard");
            document.AppendChild(root);

            document.Save(xmlPath);

            Debug.Log("Leaderboard creado: " + xmlPath);
        }
        catch (Exception e)
        {
            RegisterLeaderboardError(kahootName, "Error creando leaderboard", "No se ha podido crear el archivo XML. " + e.Message);
        }
    }

    private static void RegisterLeaderboardError(string kahootName, string title, string description)
    {
        ErrorManager.Instance.RegisterError(kahootName, title, description);
    }
}
