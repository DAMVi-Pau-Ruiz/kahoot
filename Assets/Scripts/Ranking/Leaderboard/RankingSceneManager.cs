using UnityEngine;
using TMPro;

public class RankingSceneManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Transform content;
    [SerializeField] private RankingEntryUI rankingEntryPrefab;

    private void Start()
    {
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        // Obtenemos la ruta del Kahoot seleccionado.
        string kahootPath = KahootSession.SelectedKahootPath;

        if (string.IsNullOrEmpty(kahootPath))
        {
            Debug.LogError("No se ha encontrado el Kahoot seleccionado.");
            return;
        }

        // Obtenemos los datos del Kahoot.
        KahootData kahoot = KahootSession.CurrentKahoot;

        if (kahoot != null)
        {
            titleText.text = kahoot.titulo;
        }
        else
        {
            titleText.text = "Leaderboard";
        }

        // Cargamos los resultados.
        RankingData ranking =
            RankingXMLManager.LoadRanking(kahootPath);

        if (ranking == null)
        {
            Debug.LogError("No se pudo cargar el leaderboard.");
            return;
        }

        // Ordenamos de mayor a menor puntuación.
        ranking.players.Sort(
            (a, b) => b.score.CompareTo(a.score)
        );

        // Eliminamos entradas anteriores.
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Generamos una entrada por cada jugador.
        for (int i = 0; i < ranking.players.Count; i++)
        {
            RankingPlayerData player = ranking.players[i];

            RankingEntryUI entry = Instantiate(
                rankingEntryPrefab,
                content
            );

            entry.SetData(
                i + 1,
                player.username,
                player.score
            );
        }

        Debug.Log(
            "Leaderboard cargado. Jugadores: " +
            ranking.players.Count
        );
    }
}