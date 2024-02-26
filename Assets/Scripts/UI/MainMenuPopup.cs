using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Factory;
using Logic;
using UI.Popups;
using UnityEngine;

namespace UI
{
    public class MainMenuPopup : BasePopup
    {
        private List<BasePopup> Popups => new List<BasePopup> {_workshopPopup, _cardsPopup, _battlePopup};
        private MainMenuActivePopupSetter ActivePopupSetter => _activePopupSetter ??=
            new MainMenuActivePopupSetter(Popups);

        [SerializeField] private WorkshopPopup _workshopPopup;
        [SerializeField] private CardsPopup _cardsPopup;
        [SerializeField] private BattlePopup _battlePopup;

        private MainMenuActivePopupSetter _activePopupSetter;

        public void Construct(SceneLoader sceneLoader, IGameFactory gameFactory) 
            => _battlePopup.Construct(sceneLoader, gameFactory);
        protected override void OnInitialization()
        {
            base.OnInitialization();
            Popups.ForEach(popup =>
            {
                popup.Opened += () => ActivePopupSetter.Set(popup);
                popup.gameObject.SetActive(true);
            });
        }
    }
}