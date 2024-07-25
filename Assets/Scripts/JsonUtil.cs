using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonUtil 
{
    public static string ReadJsonData(string path,string name)
    {
        var filePath = CreateDirectory(path) + "/" + name;
        return File.ReadAllText(filePath);
    }
    /// <summary>
    /// ��ȡJSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fullpath"></param>
    /// <returns></returns>
    public static T LoadJson<T>(string fullpath) where T : class
    {
        var name = fullpath.Substring(fullpath.LastIndexOf('/') + 1);
        fullpath = fullpath.Substring(0, fullpath.LastIndexOf('/'));
        return LoadJson<T>(fullpath, name);
    }

    /// <summary>
    /// ��ȡJSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T LoadJson<T> (string path,string fileName) where T : class
    {
        var filePath = CreateDirectory(path) + "/" + fileName;
        string json = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("Json ������");
            return null;
        }
        T t = JsonConvert.DeserializeObject<T>(json);
        return t;
    }

    /// <summary>
    /// ����JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="fullpath"></param>
    public static void SaveJson<T>(T t, string fullpath) where T : class
    {
        var name = fullpath.Substring(fullpath.LastIndexOf('/') + 1);
        fullpath = fullpath.Substring(0, fullpath.LastIndexOf('/'));
        SaveJson<T>(t,fullpath, name);
    }
    /// <summary>
    /// ����JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    public static void SaveJson<T>(T t,string path,string fileName) where T : class
    {
        string filePath = CreateDirectory(path) + "/" + fileName;
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }
        string json = JsonConvert.SerializeObject(t);
        File.WriteAllText(filePath,json);
        Debug.Log("Json  "+fileName+" ����ɹ�");
    }
    /// <summary>
    /// �����ļ���
    /// </summary>
    /// <param name="pathName"></param>
    /// <returns></returns>
    public static string CreateDirectory(string pathName)
    {
        //todo:�޸ĵ�ַ
        string path = Application.platform switch
        {
            RuntimePlatform.WindowsEditor => Application.streamingAssetsPath + "/" + pathName,
            RuntimePlatform.WindowsPlayer => Application.streamingAssetsPath + "/" + pathName,
            //RuntimePlatform.OSXEditor => Application.persistentDataPath + "/" + pathName,
            //RuntimePlatform.Android => Application.persistentDataPath + "/" + pathName,
            //RuntimePlatform.IPhonePlayer => Application.persistentDataPath + "/" + pathName,
            _ => Application.persistentDataPath + "/" + pathName,
        };
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    }
    /// <summary>
    /// json���л�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T Deserialize<T>(string json) where T : class
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
    /// <summary>
    /// �������л�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonObject"></param>
    /// <returns></returns>
    public static string Serialize<T>(T jsonObject) where T : class
    {
        return JsonConvert.SerializeObject(jsonObject);
    }
}

[System.Serializable]
public struct JVector3
{
    public float x;
    public float y;
    public float z;

    public JVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public JVector3(Vector3 vector3)
    {
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Quaternion ToQuaternion()
    {
        return Quaternion.Euler(x, y, z);
    }

}
[System.Serializable]
public struct JTransform
{
    public JVector3 Position;
    public JVector3 Rotation;
    public JVector3 Scale;

    public JTransform(JVector3 pos, JVector3 rot, JVector3 sca)
    {
        this.Position = pos;
        this.Rotation = rot;
        this.Scale = sca;
    }
}