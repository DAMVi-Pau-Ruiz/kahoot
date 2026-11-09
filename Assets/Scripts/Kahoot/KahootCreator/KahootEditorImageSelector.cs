using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KahootEditorImageSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image previewImage;
    [SerializeField] private TMP_Text imageNameText;
    [SerializeField] private Slider imageSlider;

    [Header("Settings")]
    [SerializeField] private string imageResourcesFolder = "KahootImages";

    private Sprite[] images;
    private int selectedIndex;

    private void Awake()
    {
        LoadImages();
    }
    private void Start()
    {
        imageSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void LoadImages()
    {
        images = Resources.LoadAll<Sprite>(imageResourcesFolder);

        imageSlider.minValue = 0;
        imageSlider.maxValue = images.Length - 1;
        imageSlider.wholeNumbers = true;

        selectedIndex = 0;

        UpdatePreview();
    }

    private void OnSliderChanged(float value)
    {
        selectedIndex = Mathf.RoundToInt(value);
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (images.Length == 0)
        {
            return;
        }

        previewImage.sprite = images[selectedIndex];
        imageNameText.text = images[selectedIndex].name;
    }

    public string GetSelectedImage()
    {
        return images[selectedIndex].name;
    }

    public void SetSelectedImage(string imageName)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].name == imageName)
            {
                selectedIndex = i;
                imageSlider.value = i;
                UpdatePreview();
                return;
            }
        }
    }
}
