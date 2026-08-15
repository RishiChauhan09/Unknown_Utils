using System.Collections.Generic;
using UnityEngine;
using Unknown.Extensions;

namespace Unknown.UI {

    public class TabGroup : MonoBehaviour {

        [System.Serializable]
        public class Tab {
            public TabButton tabButton;
            public RectTransform screen;
        }

        [SerializeField] private List<Tab> tabsInfo;

        [Header("Debugging")]
        [SerializeField] private Tab currentActiveTab;

        private void Awake() {
            foreach(Tab tab in tabsInfo) {
                tab.tabButton.Clicked += SetTabSelected;
            }
            UpdateUI();
        }

        private void SetTabSelected(TabButton btn) {

            if(btn == currentActiveTab.tabButton)
                return;

            currentActiveTab.tabButton.SetUnSelected();
            currentActiveTab.screen.SlideOutUI(Vector2.down);

            foreach(Tab tab in tabsInfo) {
                if(btn == tab.tabButton) {
                    currentActiveTab = tab;
                    tab.tabButton.SetSelected();
                    tab.screen.SetAsLastSibling();
                    tab.screen.SlideInUI(Vector2.up, ease: DG.Tweening.Ease.OutCirc);
                    break;
                }
            }
        }

        private void UpdateUI() {
            if(currentActiveTab == null)
                return;

            currentActiveTab.tabButton.SetSelected();
            currentActiveTab.screen.SlideInUI(Vector2.up, ease: DG.Tweening.Ease.OutCirc);
        }
    }
}