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
                nodeTitle = "Player selection",
                nodeConstructor = () => new EditorPlayerSelectionNode()
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

        public DialogNode GetRuntimeGraph()
        {
            IEnumerable<ThreadStartNode> startNodes = nodes.OfType<ThreadStartNode>();
            ThreadStartNode startNode = startNodes.First();
            if(startNode == null)
            {
                Debug.LogError("Dialog graph has no Thread Start");
                return null;
            }

            DialogNode startRuntimeNode = startNode.ToRuntimeNode();

            Stack<(EditorDialogNode, DialogNode)> nodesToAppendTo = new Stack<(EditorDialogNode, DialogNode)>();
            nodesToAppendTo.Push((startNode, startRuntimeNode));

            // TODO: make this cycle-safe
            while (nodesToAppendTo.Any())
            {
                (EditorDialogNode editorNode, DialogNode runtimeNode) = nodesToAppendTo.Pop();

                foreach(EditorDialogNode nextNode in editorNode.GetNextNodes())
                {
                    DialogNode nextRuntimeNode = nextNode.ToRuntimeNode();
                    nodesToAppendTo.Push((nextNode, nextRuntimeNode));
                    runtimeNode.AppendToNextNodes(nextRuntimeNode);
                }
            }

            return startRuntimeNode;
        }

        public byte[] Serialize()
        {
            Serializer serializer = new Serializer();
            byte[] nodesSerialized = serializer.SerializeNodes(nodes);

            byte[] edgesSerialized = serializer.SerializeEdges(edges);

            int requiredBytes = sizeof(int) + nodesSerialized.Length + sizeof(int) + edgesSerialized.Length;
            Serialization.BinarySerializer mergingSerializer = new Serialization.BinarySerializer(requiredBytes);
            mergingSerializer.SerializeInt(nodesSerialized.Length);
            mergingSerializer.SerializeBytes(nodesSerialized);
            mergingSerializer.SerializeInt(edgesSerialized.Length);
            mergingSerializer.SerializeBytes(edgesSerialized);

            return mergingSerializer.GetSerialization();
        }

        public void Deserialize(byte[] bytes)
        {
            DeleteElements(graphElements);

            Serialization.BinaryDeserializer splittingDeserializer = new Serialization.BinaryDeserializer(bytes);

            int nodesSerializedSize = splittingDeserializer.DeserializeInt();
            byte[] nodesSerialized = splittingDeserializer.DeserializeBytes(nodesSerializedSize);

            int edgesSerializedSize = splittingDeserializer.DeserializeInt();
            byte[] edgesSerialized = splittingDeserializer.DeserializeBytes(edgesSerializedSize);

            DeserializeNodes(nodesSerialized);
            DeserializeEdges(edgesSerialized);
        }

        private void DeserializeNodes(byte[] nodesSerialized)
        {
            List<SerializedNodeData> deserializedNodes = Serializer.DeserializeNodes(nodesSerialized);

            // Instantiate nodes
            foreach (SerializedNodeData nodeData in deserializedNodes)
            {
                EditorDialogNode node = InstantiateNodeFromType(nodeData.nodeType);
                node.Deserialize(nodeData.bytes);
                SetupNewNode(node, nodeData.nodePosition);
            }
        }

        private void DeserializeEdges(byte[] edgesSerialized)
        {
            List<(int, int)> deserializedEdges = Serializer.DeserializeEdges(edgesSerialized);

            Node[] nodesArr = nodes.ToArray();
            foreach ((int v1, int v2) in deserializedEdges)
            {
                Node inputNode = nodesArr[v1];
                Node outputNode = nodesArr[v2];

                Port inputPort = inputNode.inputContainer.Q<Port>();
                Port outputPort = outputNode.outputContainer.Q<Port>();
                if (inputPort == null || outputPort == null)
                {
                    Debug.LogError("Failed to deserialize edge");
                    continue;
                }

                Edge instantiatedEdge = outputPort.ConnectTo(inputPort);
                inputNode.RefreshPorts();
                outputNode.RefreshPorts();

                AddElement(instantiatedEdge);
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

        private Vector2 LocalToWorld(Vector2 localPos)
        {
            return ElementAt(0).worldTransform.inverse.MultiplyPoint(localPos);
        }

        private Vector2 ExtractNodePositionFromDropdownMenuAction(DropdownMenuAction a) => LocalToWorld(a.eventInfo.localMousePosition);


        private static EditorDialogNode InstantiateNodeFromType(EditorDialogNode.NodeType nodeType)
        {
            switch(nodeType)
            {
                case EditorDialogNode.NodeType.PlayerResponse:
                    return new EditorPlayerResponseNode();

                case EditorDialogNode.NodeType.NpcResponse:
                    return new EditorNpcResponseNode();

                case EditorDialogNode.NodeType.PlayerSelectionNode:
                    return new EditorPlayerSelectionNode();

                case EditorDialogNode.NodeType.ThreadStartNode:
                    return new ThreadStartNode();
            }

            throw new NotImplementedException();
        }

        private class Serializer
        {
            private Dictionary<Node, int> _nodeToIndex;

            public byte[] SerializeNodes(UQueryState<Node> nodes)
            {
                _nodeToIndex = new Dictionary<Node, int>();

                List<byte[]> serializedNodes = new List<byte[]>();
                int totalBytes = 0;

                int i = 0;
                foreach (Node node in nodes)
                {
                    EditorDialogNode dialogNode = (EditorDialogNode)node;

                    SerializedNodeData serializedData = new SerializedNodeData();
                    serializedData.bytes = dialogNode.Serialize();
                    serializedData.nodeType = dialogNode.DialogNodeType;
                    serializedData.nodePosition = dialogNode.GetPosition().position;

                    byte[] bytes = serializedData.Serialize();
                    serializedNodes.Add(bytes);
                    totalBytes += bytes.Length;

                    _nodeToIndex.Add(node, i);
                    i++;
                }

                // Merge chains
                Serialization.BinarySerializer mergingSerializer = new Serialization.BinarySerializer(totalBytes);
                foreach(byte[] bytesPart in serializedNodes)
                {
                    mergingSerializer.SerializeBytes(bytesPart);
                }
                return mergingSerializer.GetSerialization();
            }

            public static List<SerializedNodeData> DeserializeNodes(byte[] bytes)
            {
                List<SerializedNodeData> deserializedNodes = new List<SerializedNodeData>();

                int consumedBytesTotal = 0;
                while (consumedBytesTotal < bytes.Length)
                {
                    SerializedNodeData nodeData = new SerializedNodeData();

                    nodeData.Deserialize(bytes, consumedBytesTotal, out int consumedBytes);
                    consumedBytesTotal += consumedBytes;

                    deserializedNodes.Add(nodeData);
                }

                return deserializedNodes;
            }

            public byte[] SerializeEdges(UQueryState<Edge> edges)
            {
                int requiredBytes = edges.Count() * 2 * sizeof(int);
                Serialization.BinarySerializer serializer = new Serialization.BinarySerializer(requiredBytes);

                foreach (Edge edge in edges)
                {
                    serializer.SerializeInt(_nodeToIndex[edge.input.node]);
                    serializer.SerializeInt(_nodeToIndex[edge.output.node]);
                }

                return serializer.GetSerialization();
            }

            public static List<(int,int)> DeserializeEdges(byte[] bytes)
            {
                List<(int, int)> edges = new List<(int, int)>();

                Serialization.BinaryDeserializer deserializer = new Serialization.BinaryDeserializer(bytes);

                while(deserializer.GetConsumedBytes() < bytes.Length)
                {
                    int edgeInput = deserializer.DeserializeInt();
                    int edgeOutput = deserializer.DeserializeInt();
                    edges.Add((edgeInput, edgeOutput));
                }

                return edges;
            }
        }

        private struct SerializedNodeData
        {
            public byte[] bytes;
            public EditorDialogNode.NodeType nodeType;
            public Vector2 nodePosition;

            public byte[] Serialize()
            {
                Serialization.BinarySerializer serializer = new Serialization.BinarySerializer(sizeof(int) + bytes.Length + 1 + 2 * sizeof(float));

                serializer.SerializeInt(bytes.Length);
                serializer.SerializeBytes(bytes);

                serializer.SerializeByte((byte)nodeType);

                serializer.SerializeFloat(nodePosition.x);
                serializer.SerializeFloat(nodePosition.y);

                return serializer.GetSerialization();
            }

            public void Deserialize(byte[] inBytes, int startIndex, out int consumedBytes)
            {
                Serialization.BinaryDeserializer deserializer = new Serialization.BinaryDeserializer(inBytes, startIndex);

                int bytesToRead = deserializer.DeserializeInt();
                bytes = deserializer.DeserializeBytes(bytesToRead);

                nodeType = (EditorDialogNode.NodeType)deserializer.DeserializeByte();

                nodePosition.x = deserializer.DeserializeFloat();
                nodePosition.y = deserializer.DeserializeFloat();

                consumedBytes = deserializer.GetConsumedBytes();
            }
        }
    }
}
