using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public class Graph: GraphView
    {
        public Graph()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("GraphGridBackground"));

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(GenerateEntryNode());
        }

        private BaseNode GenerateEntryNode()
        {
            BaseNode baseNode = new BaseNode
            {
                title = "Entry",
                GUID = Guid.NewGuid().ToString(),
                EntryNode = true
            };

            Port generatedPort = GeneratePort(baseNode, Direction.Output);
            generatedPort.portName = @"output";
            
            baseNode.outputContainer.Add(generatedPort);

            baseNode.RefreshExpandedState();
            baseNode.RefreshPorts();

            baseNode.SetPosition(new Rect(100, 200, 200, 300));

            return baseNode;
        }

        public void CreateNode(string title)
            => AddElement(GenerateGraphNode(title));

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if(startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        private Port GeneratePort(BaseNode node, Direction portDDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDDirection, capacity, typeof(int));
        }

        public BaseNode GenerateGraphNode(string title)
        {
            BaseNode baseNode = new BaseNode
            {
                title = title,
                GUID = Guid.NewGuid().ToString()
            };

            Port inputPort = GeneratePort(baseNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "input";

            baseNode.inputContainer.Add(inputPort);

            Button addPortButton = new Button(() => { AddOutputPort(baseNode); });
            addPortButton.text = "Add Output";

            StyleColor color = new StyleColor();
            color.value = Color.gray;

            addPortButton.style.backgroundColor = color;

            baseNode.titleButtonContainer.Add(addPortButton);

            baseNode.RefreshExpandedState();
            baseNode.RefreshPorts();

            baseNode.SetPosition(new Rect(Vector2.zero, new Vector2(200, 300)));

            return baseNode;
        }

        public void AddOutputPort(BaseNode node, string overrideFileName = "")
        {
            Port outputPort = GeneratePort(node, Direction.Output);

            var oldLabel = outputPort.contentContainer.Q<Label>("type");
            outputPort.contentContainer.Remove(oldLabel);

            int portCount = node.outputContainer.Query("connector").ToList().Count;
            string portName = string.IsNullOrEmpty(overrideFileName) ? $"output {portCount}" : overrideFileName;
            

            TextField textField = new TextField
            {
                name = string.Empty,
                value = portName
            };

            textField.RegisterValueChangedCallback(value => outputPort.portName = value.newValue);
            outputPort.contentContainer.Add(new Label(" "));
            outputPort.contentContainer.Add(textField);

            var deleteButton = new Button(() => RemovePort(node, outputPort))
            {
                text = "X"
            };

            outputPort.contentContainer.Add(deleteButton);
            outputPort.portName = portName;

            node.outputContainer.Add(outputPort);

            node.RefreshExpandedState();
            node.RefreshPorts();
        }

        private void RemovePort(BaseNode node, Port port)
        {
            var edge = edges.ToList().Where(x => x.output.portName == port.portName && x.output.node == node);

            if(!edge.Any())
            {
                var foundEdge = edge.First();
                foundEdge.input.Disconnect(foundEdge);

                RemoveElement(edge.First());

                node.outputContainer.Remove(port);
                node.RefreshExpandedState();
                node.RefreshPorts();
            }
        }
    }
}
