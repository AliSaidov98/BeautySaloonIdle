using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEvents : MonoBehaviour
{
    [SerializeField] private GameObject _clientPref;
    [SerializeField] private Transform _clientSpawnPoint;
    [SerializeField] private GameObject _playerChoicePanel;
    [SerializeField] private Button _nextDayBtn;

    private Client _currentClient;
    
    public static readonly UnityEvent<Client> OnClientChoseNumber = new UnityEvent<Client>();
    public static readonly UnityEvent<int> OnChooseNumber = new UnityEvent<int>();
    public static readonly UnityEvent OnClientSpawn = new UnityEvent();
    public static readonly UnityEvent OnDayFinished = new UnityEvent();
    public static readonly UnityEvent OnSuccessService = new UnityEvent();
    public static readonly UnityEvent OnRegretService = new UnityEvent();
    
    public static bool choosingNumber;
    
    
    private const int MONEY_SUCCESS = 100;
    private const int MONEY_REGRET = 50;
    private const int REPUTATION_SUCCESS = 10;
    private const int REPUTATION_REGRET = 5;
    
    private void Awake()
    {
        OnChooseNumber.AddListener(ChooseNumber);
        OnClientChoseNumber.AddListener(ClientChoseNumber);
        OnClientSpawn.AddListener(SpawnNewClient);
        OnDayFinished.AddListener(ResetDay);
        OnSuccessService.AddListener(SuccessService);
        OnRegretService.AddListener(RegretService);
        
        _nextDayBtn.onClick.AddListener(Restart);
        
        choosingNumber = false;
        
        GameData.Init();
    }

    private void Start()
    {
        OnClientSpawn?.Invoke();
    }

    private void ChooseNumber(int number)
    {
        _playerChoicePanel.SetActive(false);
        _currentClient.GetResult(number);
        choosingNumber = false;
        GameData.ClientsServed++;
    }

    private void ClientChoseNumber(Client client)
    {
        _currentClient = client;
        _playerChoicePanel.SetActive(true);
        choosingNumber = true;
    }

    private void SpawnNewClient()
    {
        Instantiate(_clientPref, _clientSpawnPoint.position, Quaternion.identity);
    }

    private void ResetDay()
    {
        GameData.RefreshDay();
    }

    private void SuccessService()
    {
        GameData.DayReputation += REPUTATION_SUCCESS;
        GameData.MoneyEarned += MONEY_SUCCESS;
        GameData.TotalMoney += MONEY_SUCCESS;
    }

    private void RegretService()
    {
        GameData.DayReputation += REPUTATION_REGRET;
        GameData.MoneyEarned += MONEY_REGRET;
        GameData.TotalMoney += MONEY_REGRET;
    }

    private void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
