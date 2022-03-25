using DG.Tweening;
using TMPro;
using UnityEngine;

public class StatisticsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalMoneyText;
    [SerializeField] private TextMeshProUGUI _reputationText;
    [SerializeField] private TextMeshProUGUI _statisticsText;
    [SerializeField] private RectTransform _statisticsImg;
    
    private string _statistics;

    private int _clientsServed;
    private int _moneyEarned;
    private int _reputation;

    private Sequence _sequence;
    
    private void Start()
    {
        GameEvents.OnDayFinished.AddListener(UpdateDayStatistic);
        GameEvents.OnDayFinished.AddListener(ShowStatistics);
        GameEvents.OnSuccessService.AddListener(UpdateTotalStatistic);
        GameEvents.OnRegretService.AddListener(UpdateTotalStatistic);
        
        UpdateTotalStatistic();
    }

    private void ShowStatistics()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOScale(Vector3.one, 0.5f))
            .Append(_statisticsImg.DOScale(Vector3.one, 0.5f).SetDelay(1));
    }

    private void RefreshStatistics()
    {
        _clientsServed = GameData.ClientsServed;
        _moneyEarned = GameData.MoneyEarned;
        _reputation = GameData.DayReputation;
    }

    private void UpdateDayStatistic()
    {
        RefreshStatistics();
        
        _statistics = $"Клиенты {_clientsServed}" +
                      $"\n\nДеньги +{_moneyEarned}" +
                      $"\n\nРепутация {_reputation}" +
                      $"\n\nАренда -$50";
        
        _statisticsText.text = _statistics;

        GameData.ClientsServed = 0;
        GameData.MoneyEarned = 0;
    }

    private void UpdateTotalStatistic()
    {
        _totalMoneyText.text = GameData.TotalMoney.ToString();
        _reputationText.text = GameData.TotalReputation.ToString();
    }
}
