using TMPro;
using UnityEngine;

namespace rx
{
    public class ButtonCounter : MonoBehaviour
    {
        private TextMeshProUGUI textToEdit;
        private int totalClicks = 0;

        void Start()
        {
            textToEdit = GetComponentInChildren<TextMeshProUGUI>();
        }
 
        public void ButtonPressed()
        {
            totalClicks += 1;
            textToEdit.text = totalClicks.ToString();            
        }
        
    }
}