using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEditor.PackageManager;

public class Config
{
    public const string AccessKeyId = "LTAI5t9phW1SJbudsN3AHXyR";
    public const string AccessKeySecret = "bDOWqTrwDQ3QfHHSB3tEj5I5ODgOLv";
    public const string EndPoint = "oss-cn-shenzhen.aliyuncs.com";
    public const string Bucket = "unity-host-update";
}

public class LoadSendFile : MonoBehaviour
{
    static string projectRoot = Directory.GetParent(Application.dataPath).FullName;
    static string sourceDirectory = Path.Combine(projectRoot, "ServerData/StandaloneWindows64");
    static string sourceAndroidDirectory = Path.Combine(projectRoot, "ServerData/Android");

    private static OssClient ossClient;

    [MenuItem("Tools/Update AliYun")]
    public static void UpdateAliYun()
    {
        // ListFiles();
        //  SendWindowsData(); // 如果您希望在删除后立即上传数据

        SendAnriodData();
    }

    private static void SendWindowsData()
    {
        ossClient = new OssClient(Config.EndPoint, Config.AccessKeyId, Config.AccessKeySecret);

        try
        {
            // 检查源目录是否存在
            if (!Directory.Exists(sourceDirectory))
            {
                Debug.LogError($"源目录不存在: {sourceDirectory}");
                return;
            }

            // 获取目录中的所有文件
            string[] files = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                // 获取文件名
                string fileName = Path.GetFileName(filePath);

                // 计算相对路径以保持文件夹结构
                string relativePath = filePath.Substring(sourceDirectory.Length + 1); // +1 是为了去掉路径分隔符
                string objectKey = Path.Combine("StandaloneWindows64", relativePath).Replace("\\", "/"); // 替换为 OSS 的路径分隔符

                // 上传文件
                ossClient.PutObject(Config.Bucket, objectKey, filePath);
                Debug.Log("上传成功: " + objectKey);
            }
        }
        catch (OssException e)
        {
            Debug.LogError("上传报错: " + e.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("发生错误: " + ex.Message);
        }
        finally
        {
            // 可选：可以在这里释放 ossClient
        }
    }

    private static void SendAnriodData()
    {
        ossClient = new OssClient(Config.EndPoint, Config.AccessKeyId, Config.AccessKeySecret);

        try
        {
            // 检查源目录是否存在
            if (!Directory.Exists(sourceAndroidDirectory))
            {
                Debug.LogError($"源目录不存在: {sourceAndroidDirectory}");
                return;
            }

            // 获取目录中的所有文件
            string[] files = Directory.GetFiles(sourceAndroidDirectory, "*.*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                // 获取文件名
                string fileName = Path.GetFileName(filePath);

                // 计算相对路径以保持文件夹结构
                string relativePath = filePath.Substring(sourceAndroidDirectory.Length + 1); // +1 是为了去掉路径分隔符
                string objectKey = Path.Combine("Android", relativePath).Replace("\\", "/"); // 替换为 OSS 的路径分隔符

                // 上传文件
                ossClient.PutObject(Config.Bucket, objectKey, filePath);
                Debug.Log("上传成功: " + objectKey);
            }
        }
        catch (OssException e)
        {
            Debug.LogError("上传报错: " + e.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("发生错误: " + ex.Message);
        }
        finally
        {
            // 可选：可以在这里释放 ossClient
        }
    }
}
