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

        public DialogGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(CreateContextMenu));

            AddThreadStart(firstNodePos);
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
            node.title = node.GetNodeTitle();
            node.SetPosition(new Rect(position, nodeSize));

            AddElement(node);
        }

        public void AddThreadStart(Vector2 position)
        {
            CreateNode<ThreadStartNode>(position);
        }

        public void AddNpcResponse(Vector2 position)
        {
            CreateNode<EditorNpcResponseNode>(position);
        }

        public void AddPlayerChoice(Vector2 position)
        {
            CreateNode<EditorPlayerResponseNode>(position);
        }


        private void CreateContextMenu(ContextualMenuPopulateEvent e)
        {
            e.menu.ClearItems();

            e.menu.AppendAction("Add thread start", (DropdownMenuAction a) =>
            {
                Vector2 nodePos = ExtractNodePositionFromDropdownMenuAction(a);
                AddThreadStart(nodePos);
            });

            e.menu.AppendAction("Add NPC response", (DropdownMenuAction a) =>
            {
                Vector2 nodePos = ExtractNodePositionFromDropdownMenuAction(a);
                AddNpcResponse(nodePos);
            });

            e.menu.AppendAction("Add player choice", (DropdownMenuAction a) =>
            {
                Vector2 nodePos = ExtractNodePositionFromDropdownMenuAction(a);
                AddPlayerChoice(nodePos);
            });
        }

        private Vector2 ExtractNodePositionFromDropdownMenuAction(DropdownMenuAction a) => a.eventInfo.localMousePosition;
    }
}
