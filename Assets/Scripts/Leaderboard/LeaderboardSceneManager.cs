using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardSceneManager : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private LeaderboardPlayerUI playerPrefab;
    [SerializeField] private TMP_Text kahootTitleText;

    private void Start()
    {
        kahootTitleText.text = AppManager.Instance.CurrentKahoot.titulo;
        LoadPlayers();
    }

    private void LoadPlayers()
    {
        List<LeaderboardPlayer> players = LeaderboardManager.LoadLeaderboard(AppManager.Instance.CurrentKahootPath);

        for (int i = 0; i < players.Count; i++)
        {
            LeaderboardPlayerUI player = Instantiate(playerPrefab, content);

            player.SetPlayer(players[i], i + 1);
        }
    }

    public void GoToMenu()
    {
        ChangeSceneManager.Instance.GoToMainMenu();
    }
}
