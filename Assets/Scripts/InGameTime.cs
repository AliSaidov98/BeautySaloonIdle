using UnityEngine;
using UnityEngine.Events;

public class InGameTime : MonoBehaviour
{
    public static UnityEvent OnMinuteChanged = new UnityEvent();
    public static UnityEvent OnHourChanged = new UnityEvent();

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    private float minuteToRealTime;
    private float timer;

    private const float TOTAL_REAL_SECONDS = 300;
    
    private void Start()
    {
        Minute = 0;
        Hour = 10;

        minuteToRealTime = TOTAL_REAL_SECONDS / (Hour * 60);
        
        timer = minuteToRealTime;
    }

    private void Update()
    {
        if (Hour >= 20) return;

        timer -= Time.deltaTime;

        if (!(timer <= 0)) return;
        
        Minute++;
        
        if (Minute >= 60)
        {
            Hour++;
            Minute = 0;
                
            OnHourChanged?.Invoke();
        }
        
        OnMinuteChanged?.Invoke();

        timer = minuteToRealTime;
        
        if(Hour == 20)
            GameEvents.OnDayFinished?.Invoke();
    }
}
