using System.Linq;
using UnityEngine;

public class CardChange : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _cards;
    
    private GameObject _currentCard;
    private int _currentCardIndex;
    private float _backCardsScale = 0.8f;
    
    private void Awake()
    {
        _cards = _cards.Reverse().ToArray();
        _currentCard = _cards[_currentCardIndex];
        GameEvents.OnClientSpawn.AddListener(ResetAllCards);
        
        foreach (var card in _cards)
        {
            if (card != _currentCard)
            {
                SetScale(card, Vector3.one * _backCardsScale);
            }
        }
    }

    private void Update()
    {
        var distanceMoved = _currentCard.transform.localPosition.x;

        if (!(Mathf.Abs(distanceMoved) > 0)) return;
        
        var step = Mathf.SmoothStep(_backCardsScale, 1, Mathf.Abs(distanceMoved) / (Screen.width * 0.5f));

        SetScale(_currentCardIndex < _cards.Length - 1 ? _cards[_currentCardIndex + 1] : _cards[0], Vector3.one * step);
    }

    private void SetScale(GameObject card, Vector3 scale)
    {
        card.transform.localScale = scale;
    }

    public void Changed()
    {
        if (_currentCardIndex >= _cards.Length - 1)
            _currentCardIndex = 0;
        else
            _currentCardIndex++;
        
        _currentCard = _cards[_currentCardIndex];

        ReuseCard(_currentCardIndex + 1 > _cards.Length - 1 ? _cards[0] : _cards[_currentCardIndex + 1]);
    }

    private void ReuseCard(GameObject card)
    {
        if(card.activeInHierarchy) return;
        
        card.transform.SetSiblingIndex(0);
        
        card.GetComponent<SwipeEffect>().ResetCard();
    }

    private void ResetAllCards()
    {
        foreach (var card in _cards)
        {
            card.SetActive(true);
            ReuseCard(card);
            _currentCardIndex = 0;
            _currentCard = _cards[_currentCardIndex];
            
            if (card != _currentCard)
            {
                SetScale(card, Vector3.one * _backCardsScale);
            }
            else
            {
                SetScale(card, Vector3.one);
            }
        }
    }
}
