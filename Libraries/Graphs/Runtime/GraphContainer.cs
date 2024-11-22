using System;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs.Runtime
{
    [Serializable]
    public class GraphContainer: ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<BaseNodeData> BaseNodes = new List<BaseNodeData>();
    }
}
