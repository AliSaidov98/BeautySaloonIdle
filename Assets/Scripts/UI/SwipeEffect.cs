using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeEffect : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI _numberText;
    
    private Vector3 _initialPosition;
    private Color _initialColor;
    private Image _image;
    private float _distanceMoved;
    private bool _swipeLeft;

    private CardChange _cardChange;
    
    private Color _chooseColor = Color.green;
    private Color _refuseColor = Color.red;

    private void Awake()
    {
        _cardChange = GetComponentInParent<CardChange>();
        _image = GetComponent<Image>();
        _initialColor = _image.color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);

        if (transform.localPosition.x - _initialPosition.x > 0)
        {
            transform.localEulerAngles = Vector3.forward * Mathf.LerpAngle(0, -30, (_initialPosition.x + transform.localPosition.x) / (Screen.width * 0.5f));
        }
        else
        {
            transform.localEulerAngles = Vector3.forward * Mathf.LerpAngle(0, 30, (_initialPosition.x - transform.localPosition.x) / (Screen.width * 0.5f));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _initialPosition = transform.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _distanceMoved = Mathf.Abs(transform.localPosition.x - _initialPosition.x);

        if(_distanceMoved < 0.4 * Screen.width)
        {
            transform.localPosition = _initialPosition;
            transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            _swipeLeft = !(transform.localPosition.x > _initialPosition.x);
            
            StartCoroutine(MovedCard());
        }
    }

    private IEnumerator MovedCard()
    {
        float time = 0;
        var newColor = Color.white;
        
        while (newColor.a > 0)
        {
            time += Time.deltaTime;
            
            if (_swipeLeft)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x - Screen.width, time), transform.localPosition.y, 0);

                newColor = _refuseColor;
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x + Screen.width, time), transform.localPosition.y, 0);
                
                newColor = _chooseColor;
            }

            newColor.a = Mathf.SmoothStep(1, 0, 4 * time);
            _image.color = newColor;
            
            yield return null;
        }
        
        
        if (!_swipeLeft)
        {
            if(int.TryParse(_numberText.text, out var chosenNumber));
            GameEvents.OnChooseNumber?.Invoke(chosenNumber);
        }
        
        _cardChange.Changed();
        gameObject.SetActive(false);
    }

    public void ResetCard()
    {
        transform.localPosition = _initialPosition;
        transform.localEulerAngles = Vector3.zero;
        _image.color = _initialColor;
        gameObject.SetActive(true);
    }
}
