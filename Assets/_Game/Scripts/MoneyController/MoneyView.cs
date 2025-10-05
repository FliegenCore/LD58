using TMPro;
using UnityEngine;

namespace Assets._Game.Scripts.MoneyController
{
    public class MoneyView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;

        private int _moneyCount;

        public void Initialize()
        {
            _moneyText.text = $"${_moneyCount}";
        }
        
        public void RemoveMoney(int count)
        {
            //add animation
        }

        public void AddMoney(int count)
        { 
            // add animation
        }
    }
}
