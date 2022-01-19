using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Popup
{
    public class PopupDisplay : MonoBehaviour
    {
        private string _descriptionText;

        [SerializeField]
        private Text displayText;

        public string DescriptionText
        {
            get => _descriptionText;

            set
            {
                _descriptionText = value;

                if (displayText != null)
                    displayText.text = _descriptionText;
            }
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            transform.position = Mouse.current.position.ReadValue() + (Vector2.one * 5);
        }

        public void Update()
        {
            transform.position = Mouse.current.position.ReadValue() + (Vector2.one * 5);
        }
    }
}