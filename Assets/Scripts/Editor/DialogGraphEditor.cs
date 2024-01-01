using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogs
{
    public class DialogGraphEditor : EditorWindow
    {
        static string dialogFileExtension = "dlg.bytes";
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

            ToolbarMenu fileMenu = new ToolbarMenu();
            fileMenu.text = "File";
            AppendFileActions(fileMenu);
            toolbar.Add(fileMenu);

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

        private void AppendFileActions(ToolbarMenu fileMenu)
        {
            fileMenu.menu.AppendAction("Load", (DropdownMenuAction a) =>
            {
                ShowLoadGraphFile();
            });
            fileMenu.menu.AppendAction("Save", (DropdownMenuAction a) =>
            {
                ShowSaveGraphFile();
            });
        }

        private void ShowLoadGraphFile()
        {
            string filePath = EditorUtility.OpenFilePanel(title: "Select dialog graph", directory: "Assets", extension: dialogFileExtension);
            if(filePath.Length != 0)
            {
                byte[] fileContent = File.ReadAllBytes(filePath);
                graphView.Deserialize(fileContent);
            }
        }

        private void ShowSaveGraphFile()
        {
            byte[] serializedGraph = graphView.Serialize();

            string filePath = EditorUtility.SaveFilePanel(title: "New dialog graph file", directory: "Assets", defaultName: "NewDialog", extension: dialogFileExtension);

            if(filePath.Length != 0)
            {
                File.WriteAllBytes(filePath, serializedGraph);
            }
        }
    }
}