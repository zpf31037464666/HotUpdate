//
// Auto Generated Code By excel2json
// https://neil3d.gitee.io/coding/excel2json.html
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是变量名称，第二行是变量类型

// Generate From C:\Users\wasi\Desktop\BuffData.xlsx.xlsx

public class BuffData
{
	public int id; // 唯一标志
	public string buffname; // 名字
	public string BuffType; // 类型
	public string description; // 描述
	public int BuffValue; // 数值
	public string icon; // 图片路径
	public int curStack; // 层数
	public int priority; // 优先级
	public int maxStack; // 最大层数
	public string tags; // 标签
	public bool isForever; // 是否永久
	public float duration; // 持续时间
	public float tickTime; // 触发时间
	public string updateTimeType; // 更新方式
	public string removeStackUpdateType; // 移除方式
}


// End of Auto Generated Code
