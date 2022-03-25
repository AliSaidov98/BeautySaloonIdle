using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data")]
public class GameData : SingletonScriptableObject<GameData>
{
    private int reputation;
    private int totalMoney;

    public static int ClientsServed;
    public static int MoneyEarned;
    public static int TotalReputation;
    public static int DayReputation;
    public static int TotalMoney;

    private const int RENT_COST = 50;

    public static void Init()
    {
        TotalReputation = Instance.reputation;
        DayReputation = 0;
        TotalMoney = Instance.totalMoney;
        Debug.Log("Awake");
    }

    public static void RefreshDay()
    {
        DayReputation /= ClientsServed;

        TotalReputation += DayReputation;
        
        TotalMoney -= RENT_COST;
        Instance.reputation = TotalReputation;
        Instance.totalMoney = TotalMoney;
    }
    
}
