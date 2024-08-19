using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddPlayerAttackSpeedBuff : Buff
{
    public AddPlayerAttackSpeedBuff(BuffData buffData) : base(buffData)
    {
    }
    private string materialPath = "Material/Shader/Hidden_Outline.mat";
    public override void OnEnter()
    {
        Debug.Log("进入时攻击速度"+(100-buffData.BuffValue)/100f);
        ower.GetComponent<Player>()?.AddAttackSpeed((100-buffData.BuffValue)/100f);

        Addressables.LoadAssetAsync<Material>(materialPath).Completed+=(handle) =>
        {
            if (handle.Status== AsyncOperationStatus.Succeeded)
            {
                ower.GetComponent<Player>().SetMaterial(handle.Result);

                Debug.Log("改变玩家材质 buff");

            }

        };
    }
    public override void OnUpdate()
    {
    }
    public override void OnRemove()
    {
        Debug.Log("移除时攻击速度");
        ower.GetComponent<Player>()?.DecreateAttackSpeed((100-buffData.BuffValue)/100f);

        ower.GetComponent<Player>().OriginalMaterial();
    }
}
