using UnityEngine;
using UnityEngine.UIElements;

namespace Dada.Core.UI
{
    [DefaultExecutionOrder(-900)]
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;

        public VisualElement RootVisualElement { get; private set; }

        private void Awake()
        {
            if (_uiDocument == null)
                _uiDocument = GetComponent<UIDocument>();

            if (_uiDocument == null)
            {
                _uiDocument = gameObject.AddComponent<UIDocument>();
                _uiDocument.panelSettings = Resources.Load<PanelSettings>("UI/DefaultPanelSettings");
            }

            RootVisualElement = _uiDocument.rootVisualElement;
            RootVisualElement.style.flexGrow = 1;
        }
    }
}
