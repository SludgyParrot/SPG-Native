using System.IO;
using UnityEditor;
//using static Common.IO.DirectoryHelper;

namespace Editors
{
    /// <summary>
    /// Provides editor-specific functionalities for managing logs.
    /// This includes opening the logs folder and clearing logs through menu items in the Unity Editor.
    /// </summary>
    public class DebuggerEditor:  Editor
    {
        //[MenuItem("Debugger/Open Logs Folder $#O")]
        //private static void OpenLogsFolder()
        //{
        //    string logFolderDirectory = GetLogFolderDirectory();

        //    if(!Directory.Exists(logFolderDirectory))
        //    {
        //        throw new System.InvalidOperationException($"Could'nt find logs directory: {GetLogFolderDirectory()}");
        //    }

        //    if(File.Exists(GetLogFileOutputPath()))
        //    {
        //        EditorUtility.RevealInFinder(GetLogFileOutputPath());
        //    }
        //    else
        //    {
        //        EditorUtility.RevealInFinder(GetLogFolderDirectory());
        //    }
        //}

        //[MenuItem("Debugger/Clear Logs #C")]
        //private static void ClearLogs()
        //{
        //    File.Delete(GetLogFileOutputPath());

        //    UnityEngine.Debug.Log($"Logs file has been cleared from directory: {GetLogFolderDirectory()}");
        //}

        //[MenuItem("Debugger/Clear Logs #C", true)]
        //private static bool CanClearLogs()
        //{
        //    return File.Exists(GetLogFileOutputPath());
        //}
    }
}
