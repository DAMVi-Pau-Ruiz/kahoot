
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KahootImageSelector : MonoBehaviour
{
    [Header("Available Images")]
    [SerializeField] private Sprite[] availableImages;

    [Header("UI")]
    [SerializeField] private Image previewImage;
    [SerializeField] private TMP_Text imageName;
    [SerializeField] private Slider imageSlider;

    private int selectedImageIndex = 0;

    private void Start()
    {
        if (availableImages.Length == 0)
        {
            Debug.LogError("No hay imágenes disponibles.");
            return;
        }

        imageSlider.minValue = 0;
        imageSlider.maxValue = availableImages.Length - 1;
        imageSlider.wholeNumbers = true;

        imageSlider.onValueChanged.AddListener(ChangeImage);

        ChangeImage(0);
    }

    private void ChangeImage(float value)
    {
        selectedImageIndex = Mathf.RoundToInt(value);

        previewImage.sprite = availableImages[selectedImageIndex];

        imageName.text = availableImages[selectedImageIndex].name;
    }

    public string GetSelectedImage()
    {
        return availableImages[selectedImageIndex].name;
    }

    public void SetSelectedImage(string image)
    {
        for (int i = 0; i < availableImages.Length; i++)
        {
            if (availableImages[i].name == image)
            {
                imageSlider.value = i;
                ChangeImage(i);
                return;
            }
        }

        Debug.LogWarning("No se encuentra la imagen: " + image);
    }

    private void OnDestroy()
    {
        imageSlider.onValueChanged.RemoveListener(ChangeImage);
    }
}
