public interface ISaveable<T>
{
    string GetDataID();
    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    T GenerateGameData();
    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="data"></param>
    void RestoreGameData(T data);
}
