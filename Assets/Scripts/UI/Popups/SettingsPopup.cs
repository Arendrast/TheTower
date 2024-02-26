using UnityEngine;

namespace UI.Popups
{
    public class SettingsPopup : BasePopup
    {
        protected override void OnOpenPopup()
        {
            base.OnOpenPopup();
            Time.timeScale = 0;
        }

        protected override void OnClosePopup()
        {
            base.OnClosePopup();
            Time.timeScale = 1;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                OpenPopup();
        }
    }
}