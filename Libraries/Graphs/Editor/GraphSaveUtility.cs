using Graphs.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Graphs.Editor
{
    public class GraphSaveUtility
    {
        private Graph _graph;
        private GraphContainer nodeContainerCache;

        private List<Edge> Edges => _graph.edges.ToList();
        private List<BaseNode> Nodes => _graph.nodes.ToList().Cast<BaseNode>().ToList();

        public static GraphSaveUtility GetInstance(Graph graph)
        {

            return new GraphSaveUtility
            {
                _graph = graph
            };
        }

        public void Save(string fileName)
        {
            if(!Edges.Any())
            {
                return;
            }

            nodeContainerCache = ScriptableObject.CreateInstance<GraphContainer>();

            Edge[] connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

            for (int i = 0; i < connectedPorts.Length; i++)
            {
                BaseNode inputNode = connectedPorts[i].input.node as BaseNode;
                BaseNode outputNode = connectedPorts[i].output.node as BaseNode;

                nodeContainerCache.NodeLinks.Add(new NodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedPorts[i].output.portName,
                    TargetNodeGUID = inputNode.GUID
                });
            }

            foreach (var node in Nodes.Where(x => !x.EntryNode))
            {
                nodeContainerCache.BaseNodes.Add(new BaseNodeData
                {
                    GUID = node.GUID,
                    // Add custom data somewhere here.
                    Position = node.GetPosition().position
                });
            }

            string folderPath = "Assets/Resources";

            if(!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string filePath = Path.Combine(folderPath, $"{fileName}.asset");

            AssetDatabase.CreateAsset(nodeContainerCache, filePath);
            AssetDatabase.SaveAssets();
        }

        public void Load(string fileName)
        {
            nodeContainerCache = Resources.Load<GraphContainer>(fileName);

            if(nodeContainerCache == null)
            {
                EditorUtility.DisplayDialog("Graph File Not Found", $"Couldn't load graph {fileName}. File does not exist.", "Damn Ok!");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }

        private void ClearGraph()
        {
            Nodes.Find(node => node.EntryNode).GUID = nodeContainerCache.NodeLinks[0].BaseNodeGUID;

            foreach (var node in Nodes)
            {
                if(node.EntryNode)
                {
                    continue;
                }

                Edges.Where(edge => edge.input.node == node).ToList().ForEach(edge => 
                {
                    _graph.RemoveElement(edge);
                });

                _graph.RemoveElement(node);
            }
        }

        private void CreateNodes()
        {
            foreach (var nodeData in nodeContainerCache.BaseNodes)
            {
                BaseNode tempNode = _graph.GenerateGraphNode("Default Node");
                tempNode.GUID = nodeData.GUID;
                _graph.AddElement(tempNode);

                List<NodeLinkData> ports = nodeContainerCache.NodeLinks.Where(link => link.BaseNodeGUID == nodeData.GUID).ToList();

                ports.ForEach(link => _graph.AddOutputPort(tempNode, link.PortName));
            }
        }

        private void ConnectNodes()
        {

        }
    }
}
