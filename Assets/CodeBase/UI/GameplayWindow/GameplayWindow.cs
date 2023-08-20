using UnityEngine.UI;
using UnityEngine;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayWindow : Window
    {
        [field:SerializeField] public Transform UnitsParent { get; private set; }
        [field:SerializeField] public LayerMask Ground { get; private set; }
        [field:SerializeField] public Button StartWave { get; private set; }
        
        public void SetBattleState()
        {
            
        }
    }
}