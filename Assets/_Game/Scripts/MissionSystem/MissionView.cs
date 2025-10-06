using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets._Game.Scripts.MissionSystem
{
    public class MissionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _missionText;

        public TMP_Text MissionText => _missionText;
    }
}
