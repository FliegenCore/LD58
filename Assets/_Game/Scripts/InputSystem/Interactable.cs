using System.Collections;
using UnityEngine;

namespace Assets._Game.Scripts.InputSystem
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract string GetName();
        public abstract IEnumerator Interact();
    }
}
