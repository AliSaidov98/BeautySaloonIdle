using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Animate : MonoBehaviour
{
    private Animator _animator;
    private static readonly int walkStr = Animator.StringToHash("walk");
    private static readonly int idleStr = Animator.StringToHash("idle");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Idle(bool idle)
    {
        _animator.SetBool(idleStr, idle);
    }

    public void Walk(float b)
    {
        _animator.SetFloat(walkStr, b);
    }
}
