namespace settings
{
    using UnityEngine;

    public class SettingsReader : MonoBehaviour {
        
        public GenericSettings genericSettings;
        public SliderMenuSettings sliderMenuSettings;
        
        public static GenericSettings Gs;
        public static SliderMenuSettings Sms;
        
        public static SettingsReader Instance;

        void Awake(){
            
            DontDestroyOnLoad(gameObject);
            
            if(Instance==null){
                Instance=this;
            } else {
                Debug.LogWarning("A previously awakened Settings MonoBehaviour exists!", gameObject);
            }
            
            if (Gs == null)
                Gs = genericSettings;

            if (Sms == null)
                Sms = sliderMenuSettings;

        }
        
    }
}