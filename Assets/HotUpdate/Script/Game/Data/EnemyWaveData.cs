//
// Auto Generated Code By excel2json
// https://neil3d.gitee.io/coding/excel2json.html
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是变量名称，第二行是变量类型

// Generate From C:\Users\wasi\Desktop\GameDataExcel\EnemyWave.xlsx.xlsx

public class EnemyWaveData
{
    public int WaveNumber; // 敌人波数
    public string EnemyType; // 敌人类型
    public int Count; // 一次生成的数量
    public float SpawnTime; // 间隔时间
    public int RewardTime; // 持续时间
    public bool IsOverGame; // 最终波数
}
