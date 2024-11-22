using Graphs.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Graphs
{
    public class GraphWindow: EditorWindow
    {
        private Graph graphView;
        private Toolbar toolBar;

        private string fileName = "Default Node";

        [MenuItem("Graph System/Graph")]
        private static void OpenGraph()
        {
            var window = GetWindow<GraphWindow>();
            window.titleContent = new UnityEngine.GUIContent("Graph View");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolBar();
        }

        private void ConstructGraphView()
        {
            graphView = new Graph
            {
                name = "Graph"
            };

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void GenerateToolBar()
        {
            toolBar = new Toolbar();

            // Create Button
            Button nodeCreateButton = new Button(() => graphView.CreateNode("Default Node"));
            nodeCreateButton.text = "Create Node";

            // Clear Graph Button
            Button clearButton = new Button(() => { });
            clearButton.text = "Clear Graph";

            // Save Button
            Button saveButton = new Button(() => { RequestDataOperation(); });
            saveButton.text = "Save Graph";

            // Load Button
            Button loadButton = new Button(() => { RequestDataOperation(save: false); });
            loadButton.text = "Load Graph";

            TextField fileNameField = new TextField("File Name:");
            fileNameField.SetValueWithoutNotify(fileName);
            fileNameField.MarkDirtyRepaint();

            fileNameField.RegisterValueChangedCallback(onValueChanged => { fileName = onValueChanged.newValue; });

            toolBar.Add(fileNameField);
            toolBar.Add(nodeCreateButton);
            toolBar.Add(clearButton);
            toolBar.Add(saveButton);
            toolBar.Add(loadButton);

            rootVisualElement.Add(toolBar);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
            rootVisualElement.Remove(toolBar);
        }

        private void RequestDataOperation(bool save = true)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                EditorUtility.DisplayDialog("Invalid File Name!", "Please enter a fucken valid file name!!!", "Oh Shit!");
            }

            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(graphView);

            if (save)
            {
                saveUtility.Save(fileName);
            }
            else
            {
                saveUtility.Load(fileName);
            }
        }
    }
}
