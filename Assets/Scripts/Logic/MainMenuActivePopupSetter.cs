using System.Collections.Generic;
using UI;

namespace Logic
{
    public class MainMenuActivePopupSetter
    {
        private readonly List<BasePopup> _popups;

        public MainMenuActivePopupSetter(List<BasePopup> popups)
            => _popups = popups;

        public void Set(BasePopup activePopup)
        {
            foreach (var popup in _popups)
            {
                if (popup != activePopup)
                    popup.ClosePopup();
                else
                    popup.OpenPopup();
            }
        }
    }
}
