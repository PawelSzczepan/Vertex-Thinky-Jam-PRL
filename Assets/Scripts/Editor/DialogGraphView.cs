﻿using System;
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

        public byte[] Serialize()
        {
            List<byte[]> serializedNodes = new List<byte[]>();
            int totalBytes = 0;

            foreach(Node node in nodes)
            {
                EditorDialogNode dialogNode = (EditorDialogNode)node;

                SerializedNodeData serializedData = new SerializedNodeData();
                serializedData.bytes = dialogNode.Serialize();
                serializedData.nodeType = dialogNode.DialogNodeType;

                byte[] bytes = serializedData.Serialize();
                serializedNodes.Add(bytes);
                totalBytes += bytes.Length;
            }

            // Merge chains
            byte[] summedBytes = new byte[totalBytes];
            int currIdx = 0;
            foreach(byte[] bytesPart in serializedNodes)
            {
                bytesPart.CopyTo(summedBytes, currIdx);
                currIdx += bytesPart.Length;
            }

            return summedBytes;
        }

        public void Deserialize(byte[] bytes)
        {
            DeleteElements(graphElements);

            List<SerializedNodeData> deserializedNodes = new List<SerializedNodeData>();

            int consumedBytesTotal = 0;
            while(consumedBytesTotal < bytes.Length)
            {
                SerializedNodeData nodeData = new SerializedNodeData();

                nodeData.Deserialize(bytes, consumedBytesTotal, out int consumedBytes);
                consumedBytesTotal += consumedBytes;

                deserializedNodes.Add(nodeData);
            }

            // Instantiate nodes
            int i = 0;
            foreach(SerializedNodeData nodeData in deserializedNodes)
            {
                EditorDialogNode node = InstantiateNodeFromType(nodeData.nodeType);
                node.Deserialize(nodeData.bytes);
                SetupNewNode(node, firstNodePos + i * Vector2.right * 10.0f); // TODO: deserialize position

                i++;
            }
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


        private static EditorDialogNode InstantiateNodeFromType(EditorDialogNode.NodeType nodeType)
        {
            switch(nodeType)
            {
                case EditorDialogNode.NodeType.PlayerResponse:
                    return new EditorPlayerResponseNode();

                case EditorDialogNode.NodeType.NpcResponse:
                    return new EditorNpcResponseNode();

                case EditorDialogNode.NodeType.KeyboardSelectionNode:
                    return new EditorKeyboardSelectionNode(); // This will be wrong if we add other selection node types

                case EditorDialogNode.NodeType.ThreadStartNode:
                    return new ThreadStartNode();
            }

            throw new NotImplementedException();
        }

        private struct SerializedNodeData
        {
            public byte[] bytes;
            public EditorDialogNode.NodeType nodeType;
            // TODO: public Vector2 nodePosition;

            public byte[] Serialize()
            {
                byte[] outBytes = new byte[sizeof(int) + bytes.Length + sizeof(byte)];

                byte[] bytesSize = BitConverter.GetBytes(bytes.Length);
                bytesSize.CopyTo(outBytes, 0);

                bytes.CopyTo(outBytes, sizeof(int));

                outBytes[sizeof(int) + bytes.Length] = (byte)nodeType;

                return outBytes;
            }

            public void Deserialize(byte[] inBytes, int startIndex, out int consumedBytes)
            {
                int bytesToRead = BitConverter.ToInt32(inBytes, startIndex);
                consumedBytes = sizeof(int);

                bytes = new byte[bytesToRead];
                for(int i = 0; i < bytesToRead; i++)
                {
                    bytes[i] = inBytes[startIndex + consumedBytes + i];
                }
                consumedBytes += bytesToRead;

                nodeType = (EditorDialogNode.NodeType) inBytes[startIndex + consumedBytes];
                consumedBytes += sizeof(byte);
            }
        }
    }
}
