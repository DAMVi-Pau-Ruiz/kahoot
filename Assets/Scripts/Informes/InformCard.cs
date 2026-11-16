using UnityEngine;
using TMPro;

public class InformCard : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TMP_Text informTitle;
    [SerializeField] private TMP_Text kahootTitle;
    [SerializeField] private TMP_Text informDescription;
    [SerializeField] private TMP_Text date;

    public void SetInformation(ErrorLog error)
    {
        informTitle.text = error.errorTitle;
        kahootTitle.text = error.kahootName;
        informDescription.text = error.errorDescription;
        date.text = error.date;
    }
}
