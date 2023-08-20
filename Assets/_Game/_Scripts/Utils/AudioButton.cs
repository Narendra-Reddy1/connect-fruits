using UnityEngine;
using UnityEngine.UI;

namespace BenStudios
{
    [RequireComponent(typeof(Button))]
    public class AudioButton : MonoBehaviour
    {
        [SerializeField] private AudioID audioID = AudioID.ButtonClickSFX;
        private Button m_button;
        private void OnEnable()
        {
            if (TryGetComponent(out m_button))
            {
                m_button.onClick.AddListener(PlaySFX);
            }
        }
        private void OnDisable()
        {
            m_button?.onClick.RemoveListener(PlaySFX);
        }
        private void PlaySFX()
        {
            GlobalEventHandler.RequestToPlaySFX?.Invoke(audioID);
        }
    }
}