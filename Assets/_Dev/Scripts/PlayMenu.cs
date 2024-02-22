using TMPro;
using UnityEngine;

namespace _Dev.Scripts
{
    public class PlayMenu : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI killCountText;
        [SerializeField] private GameObject joyStick;
        
        
        public void SetGoldText(int goldCount)
        {
            goldText.text = goldCount.ToString();
        }

        public void SetKillCountText(int killCount)
        {
            killCountText.text = killCount.ToString();
        }
        
    }
}