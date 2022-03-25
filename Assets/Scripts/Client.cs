using System.Collections;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animate))]
public class Client : MonoBehaviour
{
    [SerializeField] private float _durationToPoint = 1;
    [SerializeField] private RectTransform _popUp;
    [SerializeField] private TextMeshProUGUI _popUpRandomText;

    private Animate _animate;
    private bool _inPoint;

    private Transform _currentNode;
    private int _currentNodeIndex;
    private Vector3[] _pathPositions;

    private int[] _randomNumMinMax = new []{1, 3};
    private int _chosenNumber;

    private bool _toExit;

    
    private void Awake()
    {
        _animate = GetComponent<Animate>();
    }

    private void OnEnable()
    {
        InitPathPositions();
        MoveByPath();
    }
    
    private void MoveByPath()
    {
        _animate.Walk(_durationToPoint);
        transform.DOPath(_pathPositions, _durationToPoint).OnWaypointChange(Rotate).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            _animate.Walk(0);
            _inPoint = true;
            ShowPopUp();
        });
    }

    private void Rotate(int index)
    {
        if(InGameTime.Hour >= 20 && !_inPoint)
            Destroy(gameObject);
        
        var lookAt = (_pathPositions[index+1] - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(lookAt);
    }

    private void InitPathPositions()
    {
        _pathPositions = new Vector3[ClientPaths.Nodes.Count];
        
        for (int i = 0; i < ClientPaths.Nodes.Count; i++)
        {
            _pathPositions[i] = ClientPaths.Nodes[i].position;
        }
    }

    private void ShowPopUp()
    {
        _popUp.DOScale(Vector3.one, 0.5f);
        _chosenNumber = Random.Range(_randomNumMinMax[0], _randomNumMinMax[1] + 1);
        _popUpRandomText.text = _chosenNumber.ToString();

        StartCoroutine(NumberSelected());
    }
    
    private void DisablePopUp()
    {
        _popUp.DOScale(Vector3.zero, 0.5f);
    }

    private IEnumerator NumberSelected()
    {
        yield return new WaitForSeconds(2);
        GameEvents.OnClientChoseNumber?.Invoke(this);
    }

    public void GetResult(int number)
    {
        DisablePopUp();
        
        if(number == _chosenNumber)
            Happy();
        else
            Sad();
        
        GoToExit();
    }

    private void Sad()
    {
        GameEvents.OnRegretService?.Invoke();
        Debug.Log("Sad");
    }

    private void Happy()
    {
        GameEvents.OnSuccessService?.Invoke();
        Debug.Log("Happy");
    }

    private void GoToExit()
    {
        _pathPositions = _pathPositions.Reverse().ToArray();

        _animate.Walk(_durationToPoint);
        transform.DOPath(_pathPositions, _durationToPoint).OnWaypointChange(Rotate).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            _animate.Walk(0);
            GameEvents.OnClientSpawn?.Invoke();
            Destroy(gameObject);
        });
    }
}

