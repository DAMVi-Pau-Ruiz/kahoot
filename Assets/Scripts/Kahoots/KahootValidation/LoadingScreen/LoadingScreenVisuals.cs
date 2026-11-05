
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenVisuals : MonoBehaviour
{
    [Header("Logo")]
    [SerializeField] private Image rotatingLogo;
    [SerializeField] private float rotatingLogoSpeed = 120f;

    [Header("Background Shapes")]
    [SerializeField] private RectTransform backgroundShapes;
    [SerializeField] private float movementRadiusX = 80f;
    [SerializeField] private float movementRadiusY = 60f;
    [SerializeField] private float movementSpeed = 0.15f;
    [SerializeField] private float irregularity = 0.35f;

    [Header("Loading Text")]
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private string loadingMessage = "Loading...";
    [SerializeField] private float letterDelay = 0.2f;

    private float letterTimer;
    private int currentLetter;

    private Vector2 initialPosition;
    private float randomOffset;

    private void Start()
    {
        initialPosition = backgroundShapes.anchoredPosition;
        randomOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        RotateLogo();
        MoveBackgroundShapes();
        AnimateLoadingText();
    }

    private void RotateLogo()
    {
        rotatingLogo.transform.Rotate(
            0,
            0,
            -rotatingLogoSpeed * Time.deltaTime
        );
    }

    private void MoveBackgroundShapes()
    {
        float time = Time.time * movementSpeed + randomOffset;

        float x = Mathf.Cos(time) +
                  Mathf.Sin(time * 2.3f) * irregularity;

        float y = Mathf.Sin(time) +
                  Mathf.Cos(time * 1.7f) * irregularity;

        Vector2 offset = new Vector2(
            x * movementRadiusX,
            y * movementRadiusY
        );

        backgroundShapes.anchoredPosition = initialPosition + offset;
    }

    private void AnimateLoadingText()
    {
        letterTimer += Time.deltaTime;

        if (letterTimer >= letterDelay)
        {
            currentLetter++;

            loadingText.text = loadingMessage.Substring(0, currentLetter);

            letterTimer = 0;

            if (currentLetter >= loadingMessage.Length)
            {
                currentLetter = 0;
            }
        }
    }
}
