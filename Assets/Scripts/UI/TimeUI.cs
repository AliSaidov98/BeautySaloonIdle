using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timeText;

    private void OnEnable()
    {
        InGameTime.OnMinuteChanged.AddListener(UpdateTime);
    }

    private void UpdateTime()
    {
        _timeText.text = $"{InGameTime.Hour} : {(InGameTime.Minute >= 10 ? InGameTime.Minute.ToString() : $"0{InGameTime.Minute}")}";
    }
}
