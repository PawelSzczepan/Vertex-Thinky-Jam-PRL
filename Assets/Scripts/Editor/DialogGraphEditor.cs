using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
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
            CreateGraphView();
            CreateToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }


        private void CreateGraphView()
        {
            graphView = new DialogGraphView
            {
                name = "Dialog Graph"
            };

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            rootVisualElement.Add(toolbar);

            ToolbarMenu addMenu = new ToolbarMenu();
            addMenu.text = "Add";
            AppendAddingActions(addMenu);
            toolbar.Add(addMenu);
        }

        private void AppendAddingActions(ToolbarMenu addMenu)
        {
            Vector2 newNodePos = new Vector2(0, 0);

            foreach(DialogGraphView.NodeCreationCommand creationCommand in graphView.CreationCommands)
            {
                addMenu.menu.AppendAction(creationCommand.nodeTitle, (DropdownMenuAction a) =>
                {
                    graphView.CreateNode(creationCommand, newNodePos);
                });
            }
        }
    }
}