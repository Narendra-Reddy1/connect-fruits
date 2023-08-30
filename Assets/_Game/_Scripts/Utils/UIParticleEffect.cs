using UnityEngine;


namespace Deftouch
{
    public class UIParticleEffect : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject uiParticle;
        [SerializeField] private bool enableParticle = true;
        #endregion

        #region UnityMethods
        void Start()
        {
            EnableAndDisableEffect(enableParticle);
        }

        #endregion

        #region PublicMethods
        public void EnableAndDisableEffect(bool value = false)
        {
            //if (GlobalVariables.enableUIEffects)
            //    uiParticle.SetActive(value);
            //else
            //    uiParticle.SetActive(false);

        }
        #endregion

        #region PrivateMethods

        #endregion

        #region GameEventListeners

        #endregion
    }
}
