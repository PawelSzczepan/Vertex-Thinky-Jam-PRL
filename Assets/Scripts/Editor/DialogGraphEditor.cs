using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogs
{
    public class DialogGraphEditor : EditorWindow
    {
        static Color backgroundColor = new Color(0.1f, 0.1f, 0.15f);

        private DialogGraphView graphView;
    

        [MenuItem("Dialogs/Dialog Graph Editor")]
        public static void ShowExample()
        {
            DialogGraphEditor wnd = GetWindow<DialogGraphEditor>();
            wnd.titleContent = new GUIContent("Dialog Graph Editor");
        }

        public void CreateGUI()
        {
            rootVisualElement.style.backgroundColor = new StyleColor(backgroundColor);
        }

        private void OnEnable()
        {
            graphView = new DialogGraphView
            {
                name = "Dialog Graph"
            };

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
    }

}