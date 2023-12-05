using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace UI
{
    /// <summary>UI list with discovered story elements.</summary>
    [RequireComponent(typeof(UIDocument))]
    public class StoryElementListView : MonoBehaviour
    {
        [field: SerializeField] private VisualTreeAsset StoryElementListEntryView { get; set; }
        
        private UIDocument uiDocument;
        private ListView listView;
        private List<IStoryElementInfoProvider> StoryElements { get; } = new();
        private const bool ShouldAddDebugStoryElements = true;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            listView = uiDocument.rootVisualElement.Q<ListView>();

            if (ShouldAddDebugStoryElements)
            {
                foreach (var _ in Enumerable.Range(0, 10))
                {
                    StoryElements.Add(new DefaultStoryElementInfoProvider());
                }
            }
        }

        private void Start()
        {
            Assert.IsNotNull(StoryElementListEntryView);
            
            listView.makeItem = () => StoryElementListEntryView.Instantiate();
            listView.bindItem = (visual, index) => visual.Q<Label>().text = StoryElements[index].GetInfo().Name;
            listView.itemsSource = StoryElements;
            listView.selectionType = SelectionType.Single;
        }
    }
}
