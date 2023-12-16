using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Dialogs
{
    public class DialogGraphView : GraphView
    {
        private static Vector2 firstNodePos = new Vector2(100, 200);
        private static Vector2 nodeSize = new Vector2(100, 150);

        public struct NodeCreationCommand
        {
            public string nodeTitle;
            public Func<EditorDialogNode> nodeConstructor;
        }

        public List<NodeCreationCommand> CreationCommands { get; private set; } = new List<NodeCreationCommand>
        { // Tutaj dodajemy opisy wszystkich dostępnych komend stworzenia node'ów
            new NodeCreationCommand
            {
                nodeTitle = "Thread start",
                nodeConstructor = () => new ThreadStartNode()
            },
            new NodeCreationCommand
            {
                nodeTitle = "Npc response",
                nodeConstructor = () => new EditorNpcResponseNode()
            },
            new NodeCreationCommand
            {
                nodeTitle = "Player response",
                nodeConstructor = () => new EditorPlayerResponseNode()
            },
            new NodeCreationCommand
            {
                nodeTitle = "Keyboard selection",
                nodeConstructor = () => new EditorKeyboardSelectionNode()
            }
        };

        public DialogGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(CreateContextMenu));

            CreateNode<ThreadStartNode>(firstNodePos);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            EditorDialogNode startNode = (EditorDialogNode)startPort.node;

            List<Port> compatibles = new List<Port>();

            foreach(Port port in ports)
            {
                EditorDialogNode node = (EditorDialogNode)port.node;

                if (node == startNode)
                    continue;

                if (node.DialogNodeType == startNode.DialogNodeType)
                    continue;

                if (port.direction == startPort.direction)
                    continue;

                compatibles.Add(port);
            }

            return compatibles;
        }

        public void CreateNode<T>(Vector2 position) 
            where T : EditorDialogNode, new() // Tylko nieabstrakcyjne EditorDialogNode'y
        {
            T node = new T();
            SetupNewNode(node, position);
        }

        public void CreateNode(NodeCreationCommand command, Vector2 position)
        {
            EditorDialogNode node = command.nodeConstructor();
            SetupNewNode(node, position);
        }

        private void SetupNewNode(EditorDialogNode node, Vector2 position)
        {
            node.title = node.GetNodeTitle();
            node.SetPosition(new Rect(position, nodeSize));

            AddElement(node);
        }


        private void CreateContextMenu(ContextualMenuPopulateEvent e)
        {
            e.menu.ClearItems();

            foreach(NodeCreationCommand creationCommand in CreationCommands)
            {
                e.menu.AppendAction($"Add {creationCommand.nodeTitle}", (DropdownMenuAction a) =>
                {
                    Vector2 nodePos = ExtractNodePositionFromDropdownMenuAction(a);
                    CreateNode(creationCommand, nodePos);
                });
            }
        }

        private Vector2 ExtractNodePositionFromDropdownMenuAction(DropdownMenuAction a) => a.eventInfo.localMousePosition;
    }
}
