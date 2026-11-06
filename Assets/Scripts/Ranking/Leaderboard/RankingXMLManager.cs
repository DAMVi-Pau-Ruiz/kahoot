using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public static class RankingXMLManager
{
    private static string rankingsFolder = Path.Combine(
        Application.persistentDataPath,
        "Rankings"
    );

    // Generamos un identificador único a partir de la ruta del Kahoot.
    private static string GetRankingPath(string kahootPath)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(kahootPath);
            byte[] hash = sha256.ComputeHash(bytes);

            string id = BitConverter.ToString(hash)
                .Replace("-", "")
                .Substring(0, 16);

            return Path.Combine(rankingsFolder, id + ".xml");
        }
    }

    // Guarda un nuevo resultado.
    public static bool SaveResult(
        string kahootPath,
        string username,
        int score
    )
    {
        if (string.IsNullOrWhiteSpace(kahootPath) ||
            string.IsNullOrWhiteSpace(username))
        {
            Debug.LogError("Faltan datos para guardar el resultado.");
            return false;
        }

        try
        {
            Directory.CreateDirectory(rankingsFolder);

            string path = GetRankingPath(kahootPath);

            RankingData ranking;

            if (File.Exists(path))
            {
                ranking = LoadRanking(kahootPath);

                if (ranking == null)
                    return false;
            }
            else
            {
                ranking = new RankingData();
            }

            RankingPlayerData newPlayer = new RankingPlayerData
            {
                username = username,
                score = score
            };

            ranking.players.Add(newPlayer);

            // Ordenamos de mayor a menor puntuación.
            ranking.players.Sort(
                (a, b) => b.score.CompareTo(a.score)
            );

            XmlSerializer serializer =
                new XmlSerializer(typeof(RankingData));

            using (FileStream stream = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write))
            {
                serializer.Serialize(stream, ranking);
            }

            Debug.Log("Leaderboard guardado correctamente: " + path);

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "Error guardando el leaderboard: " +
                exception.Message
            );

            return false;
        }
    }

    // Carga el leaderboard de un Kahoot.
    public static RankingData LoadRanking(string kahootPath)
    {
        string path = GetRankingPath(kahootPath);

        if (!File.Exists(path))
            return new RankingData();

        try
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(RankingData));

            using (FileStream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read))
            {
                return (RankingData)serializer.Deserialize(stream);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "Error cargando el leaderboard: " +
                exception.Message
            );

            return null;
        }
    }
}