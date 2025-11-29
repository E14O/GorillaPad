using GorillaPad.Functions.UI;
using GorillaPad.Interfaces;
using UnityEngine;

namespace GorillaPad.Functions.Apps
{
    internal class CreditsApp : AppModule
    {
        public override string AppName => "Credits";
        public override string AppVersion => "0.0.1";

        private int _currentPage;
        private GameObject _pageOneObj;
        private GameObject _pageTwoObj;

        public override void OnAppOpen()
        {
            base.OnAppOpen();
            AppContent();
        }

        public override void AppContent()
        {
            base.AppContent();

            _pageOneObj = PadHandler.instance.AppInterfaces.transform.Find($"{AppName}App/PageOne").gameObject;
            _pageTwoObj = PadHandler.instance.AppInterfaces.transform.Find($"{AppName}App/PageTwo").gameObject;
            _currentPage = 0;

            RefreshCreditsPage();

            Transform parent = PadHandler.instance.AppInterfaces.transform.Find($"{AppName}App");
            PadButton.Create(parent, "Background", SelectedAudio.ButtonAudio, ButtonClicked);
        }

        public void ButtonClicked()
        {
            _currentPage = (_currentPage == 0) ? 1 : 0;
            RefreshCreditsPage();
        }

        private void RefreshCreditsPage()
        {
            if (_pageOneObj != null && _pageTwoObj != null)
            {
                _pageOneObj.SetActive(_currentPage == 0);
                _pageTwoObj.SetActive(_currentPage == 1);
            }
        }
    }
}

