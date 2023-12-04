using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace General
{
    public class UpgradesPanel : MonoBehaviour
    {
        [SerializeField] private List<Image> _listOfPanels = new List<Image>();

        public void SwitchPanel(Image requiredPanel)
        {
            foreach (var panel in _listOfPanels)
                panel.gameObject.SetActive(false);
            Debug.Log(1);
            requiredPanel.gameObject.SetActive(true);
            Debug.Log(2);
        }
    }
}
