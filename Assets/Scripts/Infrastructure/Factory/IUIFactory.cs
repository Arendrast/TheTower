using Infrastructure.Services;
using UI;

namespace Infrastructure.Factory
{
    public interface IUIFactory : IService
    {
        public MainMenuPopup CreateMainMenuPopup();
    }
}