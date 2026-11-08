
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarouselManager : MonoBehaviour
{
    [Header("Game Modes")]
    [SerializeField] private List<ModeData> gameModes = new List<ModeData>();

    [Header("Carousel References")]
    [SerializeField] private CarouselCard cardPrefab;
    [SerializeField] private Transform cardsContainer;

    [Header("Carousel Settings")]
    [SerializeField] private float padding = 40f;
    [SerializeField] private float sideScale = 0.75f;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.3f;

    private List<CarouselCard> spawnedCards = new List<CarouselCard>();

    private int focusedIndex = 0;
    private bool isAnimating = false;

    private void Start()
    {
        GenerateCards();
        UpdateCarousel(false);
    }

    private void GenerateCards()
    {
        foreach (ModeData mode in gameModes)
        {
            CarouselCard newCard = Instantiate(cardPrefab, cardsContainer);

            newCard.SetData(mode);

            newCard.SetButtonAction(() => OpenMode(mode));

            spawnedCards.Add(newCard);
        }
    }

    private void OpenMode(ModeData mode)
    {
        Debug.Log("BOTÓN PULSADO: " + mode.modeName);

        switch (mode.modeType)
        {
            case GameModeType.PLAY:
                KahootSession.SelectionMode = KahootSelectionMode.PLAY;
                break;

            case GameModeType.EDIT:
                KahootSession.SelectionMode = KahootSelectionMode.EDIT;
                break;

            case GameModeType.RANKING:
                KahootSession.SelectionMode = KahootSelectionMode.RANKING;
                break;
        }

        Debug.Log("MODO ASIGNADO: " + KahootSession.SelectionMode);

        SceneManager.LoadScene(mode.sceneName);
    }

    private void UpdateCarousel(bool animate = true)
    {
        if (spawnedCards.Count == 0)
            return;

        float cardWidth = cardPrefab.GetComponent<RectTransform>().sizeDelta.x;
        float spacing = cardWidth + padding;

        spawnedCards[focusedIndex].transform.SetAsLastSibling();

        if (animate)
        {
            StartCoroutine(AnimateCarousel(spacing));
        }
        else
        {
            for (int i = 0; i < spawnedCards.Count; i++)
            {
                int relativePosition = i - focusedIndex;

                RectTransform cardTransform =
                    spawnedCards[i].GetComponent<RectTransform>();

                cardTransform.anchorMin = new Vector2(0.5f, 0.5f);
                cardTransform.anchorMax = new Vector2(0.5f, 0.5f);
                cardTransform.pivot = new Vector2(0.5f, 0.5f);

                cardTransform.anchoredPosition =
                    new Vector2(relativePosition * spacing, 0);

                cardTransform.localScale = i == focusedIndex
                    ? Vector3.one
                    : Vector3.one * sideScale;
            }
        }
    }

    private IEnumerator AnimateCarousel(float spacing)
    {
        isAnimating = true;

        List<Vector2> startPositions = new List<Vector2>();
        List<Vector3> startScales = new List<Vector3>();

        List<Vector2> targetPositions = new List<Vector2>();
        List<Vector3> targetScales = new List<Vector3>();

        for (int i = 0; i < spawnedCards.Count; i++)
        {
            RectTransform cardTransform =
                spawnedCards[i].GetComponent<RectTransform>();

            int relativePosition = i - focusedIndex;

            startPositions.Add(cardTransform.anchoredPosition);
            startScales.Add(cardTransform.localScale);

            targetPositions.Add(
                new Vector2(relativePosition * spacing, 0)
            );

            targetScales.Add(
                i == focusedIndex
                    ? Vector3.one
                    : Vector3.one * sideScale
            );
        }

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // SmoothStep
            float smoothT = t * t * (3f - 2f * t);

            for (int i = 0; i < spawnedCards.Count; i++)
            {
                RectTransform cardTransform =
                    spawnedCards[i].GetComponent<RectTransform>();

                cardTransform.anchoredPosition = Vector2.Lerp(
                    startPositions[i],
                    targetPositions[i],
                    smoothT
                );

                cardTransform.localScale = Vector3.Lerp(
                    startScales[i],
                    targetScales[i],
                    smoothT
                );
            }

            yield return null;
        }

        for (int i = 0; i < spawnedCards.Count; i++)
        {
            RectTransform cardTransform =
                spawnedCards[i].GetComponent<RectTransform>();

            cardTransform.anchoredPosition = targetPositions[i];
            cardTransform.localScale = targetScales[i];
        }

        isAnimating = false;
    }

    public void Next()
    {
        if (isAnimating || focusedIndex >= spawnedCards.Count - 1)
            return;

        focusedIndex++;
        UpdateCarousel();
    }

    public void Previous()
    {
        if (isAnimating || focusedIndex <= 0)
            return;

        focusedIndex--;
        UpdateCarousel();
    }

    private void OpenMode(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
