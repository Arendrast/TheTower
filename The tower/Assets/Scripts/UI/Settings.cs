using System;
using System.Collections.Generic;
using General;
using UnityEngine;

namespace UI
{
    public class Settings : Switchable
    {
        public override void SetState()
        {
            base.SetState();
            Time.timeScale = Convert.ToInt32(!IsOpen);  
        } 
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                Open();
        }

        private void Open()
        {
            if (!IsOpen)
                SetState();
        }
    }
}