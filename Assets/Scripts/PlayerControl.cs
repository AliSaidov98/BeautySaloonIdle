using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animate))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform _playerReceptionPosition;
    [SerializeField] private float _rotationSpeed = 20;
    
    private NavMeshAgent _navMeshAgent;
    private Camera _cam;
    private Animate _anim;

    private int _tapCount;
    
    private bool _goToReception;
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animate>();
        _cam = Camera.main;
    }

    private void Update()
    {
        MoveToPoint();
    }

    private void MoveToPoint()
    {
        RotatePlayer();
        
        if(GameEvents.choosingNumber) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            _tapCount += 1;
            StartCoroutine(Countdown());    
        }

        if (_tapCount != 2) return;
        
        _tapCount = 0;
        StopCoroutine(Countdown());
            
        var mousePosition = _cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(mousePosition, out var hitData, 1000)) return;
        
        if(hitData.transform.CompareTag(Tags.Reception_Tag))
            GoToReception();
        else
            _navMeshAgent.SetDestination(hitData.point);
    }

    private void RotatePlayer()
    {
        if ((Math.Abs(_navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance) > 0f))
        {
            _anim.Walk(_navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance);

            var lookAt = (_navMeshAgent.steeringTarget - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt),
                Time.deltaTime * _rotationSpeed);
        }
        else if (Math.Abs(_navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance) < 0.1 &&
                 _goToReception)
        {
            transform.rotation = _playerReceptionPosition.rotation;
            _goToReception = false;
        }

    }

    private void GoToReception()
    {
        _navMeshAgent.SetDestination(_playerReceptionPosition.position);
        _goToReception = true;
    }
    
    private IEnumerator Countdown()
    { 
        yield return new WaitForSeconds(0.3f);
        _tapCount = 0;  
    }
}
