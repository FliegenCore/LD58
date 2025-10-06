using UnityEngine;

public class ChildAnimationStarter : MonoBehaviour
{
    [SerializeField] private string _animationName;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        StartAnimation();
    }

    private void StartAnimation()
    {
        _animator.Play(_animationName);
    }
}
