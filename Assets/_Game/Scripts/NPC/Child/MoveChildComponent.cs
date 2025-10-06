using System.Collections;
using UnityEngine;

namespace Assets._Game.Scripts.NPC.Child
{
    internal class MoveChildComponent : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private bool _backward;

        private bool _enabled = true;
        private int _currentPointIndex = 0;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!_enabled || _points.Length == 0)
                return;

            Transform targetPoint = _points[_currentPointIndex];

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                _moveSpeed * Time.deltaTime
            );

            if (transform.position != targetPoint.position)
            {
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    if(_backward)
                    {
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    else
                    {
                        transform.rotation = Quaternion.LookRotation(-direction);
                    }
                }
            }

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                _currentPointIndex++;

                if (_currentPointIndex >= _points.Length)
                {
                    _currentPointIndex = 0;
                }
            }
        }

        public void DoCrashEvent()
        {
            DisableMove();
            DoIdleAnimation();

            //start dialogue
        }

        private void DisableMove()
        {

        }

        private void DoIdleAnimation()
        {

        }

        private IEnumerator WaitMoveEnable()
        {
            yield return new WaitForSeconds(2f);
        }
    }
}
