using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        public static OptionsMenu Instance;

        public AudioMixer mixer;
        public AudioSetting[] audioSettings;

        private enum AudioGroups { Master, Music, SFX };

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);

            Instance = this;
        }

        void Start()
        {
            //if (Instance != this) return;
            for (int i = 0; i < audioSettings.Length; i++)
            {
                audioSettings[i].Initialize();
            }

        }

        public void SetMasterVolume(float value)
        {
            float trueValue = value;
            if (value <= -40) trueValue = -80f;
            audioSettings[(int)AudioGroups.Master].SetExposedParam(trueValue);

        }

        public void SetMusicVolume(float value)
        {
            float trueValue = value;
            if (value <= -40) trueValue = -80f;
            audioSettings[(int)AudioGroups.Music].SetExposedParam(trueValue);

        }

        public void SetSFXVolume(float value)
        {
            float trueValue = value;
            if (value <= -40) trueValue = -80f;
            audioSettings[(int)AudioGroups.SFX].SetExposedParam(trueValue);

        }

        public void SetWindowedMode(bool value)
        {
            Screen.fullScreen = value;
        }
    }

    [System.Serializable]
    public class AudioSetting
    {
        [SerializeField]
        private string groupName;
        public Slider slider;
        //public GameObject redX;
        public string exposedParam;

        public void Initialize()
        {
            slider.value = PlayerPrefs.GetFloat(exposedParam, 0);
        }

        public void SetExposedParam(float value) // 1
        {
            //redX.SetActive(value <= slider.minValue); // 2
            OptionsMenu.Instance.mixer.SetFloat(exposedParam, value); // 3
            PlayerPrefs.SetFloat(exposedParam, value); // 4
        }
    }
}