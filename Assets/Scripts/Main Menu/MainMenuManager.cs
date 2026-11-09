using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private List<MainMenuOption> options = new List<MainMenuOption>();

    [Header("References")]
    [SerializeField] private MainMenuCard optionPrefab;
    [SerializeField] private Transform optionsContainer;

    [Header("Carousel Settings")]
    [SerializeField] private float spacing = 40;
    [SerializeField] private float sideScale = 0.75f;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.3f;

    private List<MainMenuCard> spawnedOptions = new List<MainMenuCard>();

    private int selectedIndex = 0;
    private bool isAnimating = false;


    private void Start()
    {
        GenerateOptions();

        selectedIndex = GetInitialSelectedIndex();

        UpdateCarousel(false);
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            Next();
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            Previous();
        }
    }

    private void GenerateOptions()
    {
        foreach (MainMenuOption option in options)
        {
            MainMenuCard newOption = Instantiate(optionPrefab, optionsContainer);
            newOption.SetData(option, SelectOption);

            spawnedOptions.Add(newOption);
        }
    }

    private void UpdateCarousel(bool animate = true)
    {
        if (spawnedOptions.Count == 0)
        {
            return;
        }

        float cardWith = optionPrefab.GetComponent<RectTransform>().rect.width;
        float totalSpacing = cardWith + spacing;

        spawnedOptions[selectedIndex].transform.SetAsLastSibling();

        if (animate)
        {
            StartCoroutine(AnimateCarousel(totalSpacing));
        }
        else
        {
            for (int i = 0; i < spawnedOptions.Count; i++)
            {
                int relativePosition = i - selectedIndex;

                RectTransform cardTransform = spawnedOptions[i].GetComponent<RectTransform>();

                cardTransform.anchoredPosition = new Vector2(relativePosition * totalSpacing, 0);

                cardTransform.localScale = i == selectedIndex ? Vector3.one : Vector3.one * sideScale;
            }
        }
    }

    private IEnumerator AnimateCarousel(float totalSpacing)
    {
        isAnimating = true;

        List<Vector2> startPositions = new List<Vector2>();
        List<Vector3> startScales = new List<Vector3>();

        List<Vector2> targetPositions = new List<Vector2>();
        List<Vector3> targetScales = new List<Vector3>();

        for (int i = 0; i < spawnedOptions.Count; i++)
        {
            RectTransform cardTransform =
                spawnedOptions[i].GetComponent<RectTransform>();

            int relativePosition = i - selectedIndex;

            startPositions.Add(cardTransform.anchoredPosition);
            startScales.Add(cardTransform.localScale);

            targetPositions.Add(new Vector2(
                relativePosition * totalSpacing,
                0
            ));

            targetScales.Add(i == selectedIndex
                ? Vector3.one
                : Vector3.one * sideScale);
        }

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // SmoothStep
            float smoothT = t * t * (3f - 2f * t);

            for (int i = 0; i < spawnedOptions.Count; i++)
            {
                RectTransform cardTransform =
                    spawnedOptions[i].GetComponent<RectTransform>();

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

        for (int i = 0; i < spawnedOptions.Count; i++)
        {
            RectTransform cardTransform =
                spawnedOptions[i].GetComponent<RectTransform>();

            cardTransform.anchoredPosition = targetPositions[i];
            cardTransform.localScale = targetScales[i];
        }

        isAnimating = false;
    }

    public void Next()
    {
        if (isAnimating || selectedIndex >= spawnedOptions.Count - 1)
        {
            return;
        }

        selectedIndex++;
        AudioManager.Instance.PlaySFX(SFXType.SLIDE);
        UpdateCarousel();
    }

    public void Previous()
    {
        if (isAnimating || selectedIndex <= 0)
        {
            return;
        }

        selectedIndex--;
        AudioManager.Instance.PlaySFX(SFXType.SLIDE);
        UpdateCarousel();
    }

    private void SelectOption(AppAction action)
    {
        AppManager.Instance.SetAction(action);

        switch (action)
        {
            case AppAction.PLAY:
                ChangeSceneManager.Instance.GoToUsername();
                break;

            case AppAction.LEADERBOARD:
                ChangeSceneManager.Instance.GoToKahootSelector();
                break;

            case AppAction.EDITOR:
                ChangeSceneManager.Instance.GoToEditorMenu();
                break;

            case AppAction.EXIT:
                //exits the app
                break;
        }
    }

    private int GetInitialSelectedIndex()
    {
        for (int i = 0; i < spawnedOptions.Count; i++)
        {
            if (options[i].action == AppManager.Instance.CurrentAction)
            {
                return i;
            }
        }

        return 0;
    }
}
