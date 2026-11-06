using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenVisuals : MonoBehaviour
{
    [Header("Logo")]
    [SerializeField] private Image rotatingLogo;
    [SerializeField] private float rotatingLogoSpeed = 120f;

    [Header("Loading Text")]
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private string loadingMessage = "Loading...";
    [SerializeField] private float letterDelay = 0.2f;

    private float letterTimer;
    private int currentLetter;

    private void Update()
    {
        RotateLogo();
        AnimateLoadingText();
    }

    private void RotateLogo()
    {
        if (rotatingLogo == null)
        {
            return;
        }

        rotatingLogo.transform.Rotate(
            0,
            0,
            -rotatingLogoSpeed * Time.deltaTime
        );
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
