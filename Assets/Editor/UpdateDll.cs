using System.IO;
using UnityEditor;
using UnityEngine;

public class FileRenameAndCopy : MonoBehaviour
{
    [MenuItem("Tools/Rename HotUpdate.dll to HotUpdate.dll.bytes and Copy Window")]
    public static void RenameAndCopyFile()
    {
        // 获取项目根目录
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;

        // 源文件路径
        string sourcePath = Path.Combine(projectRoot, "HybridCLRData/HotUpdateDlls/StandaloneWindows64/HotUpdate.dll");

        // 目标重命名后的文件路径
        string targetPath = Path.Combine(projectRoot, "HybridCLRData/HotUpdateDlls/StandaloneWindows64/HotUpdate.dll.bytes");

        // 复制目标文件路径
        string copyDestinationPath = Path.Combine(projectRoot, "Assets/HotUpdate/Dlls/HotUpdate.dll.bytes");

        // 检查源文件是否存在
        if (File.Exists(sourcePath))
        {
            // 重命名文件
            File.Move(sourcePath, targetPath);
            Debug.Log($"文件已重命名为: {targetPath}");

            // 复制重命名后的文件到指定目录
            File.Copy(targetPath, copyDestinationPath, overwrite: true);
            Debug.Log($"文件已复制到: {copyDestinationPath}");
        }
        else
        {
            Debug.LogError($"源文件不存在: {sourcePath}");
        }
    }
    [MenuItem("Tools/Rename HotUpdate.dll to HotUpdate.dll.bytes and Copy Android")]
    public static void RenameAndCopyFileAndroid()
    {
        // 获取项目根目录
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;

        // 源文件路径
        string sourcePath = Path.Combine(projectRoot, "HybridCLRData/HotUpdateDlls/Android/HotUpdate.dll");

        // 目标重命名后的文件路径
        string targetPath = Path.Combine(projectRoot, "HybridCLRData/HotUpdateDlls/Android/HotUpdate.dll.bytes");

        // 复制目标文件路径
        string copyDestinationPath = Path.Combine(projectRoot, "Assets/HotUpdate/Dlls/HotUpdate.dll.bytes");

        // 检查源文件是否存在
        if (File.Exists(sourcePath))
        {
            // 重命名文件
            File.Move(sourcePath, targetPath);
            Debug.Log($"文件已重命名为: {targetPath}");

            // 复制重命名后的文件到指定目录
            File.Copy(targetPath, copyDestinationPath, overwrite: true);
            Debug.Log($"文件已复制到: {copyDestinationPath}");
        }
        else
        {
            Debug.LogError($"源文件不存在: {sourcePath}");
        }
    }
}
