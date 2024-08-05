using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHealthBuffSkill : Skill
{
    public TakeHealthBuffSkill(SkillData skillData) : base(skillData)
    {
    }
    public override void Apply()
    {
        if (coolDownTime!=0) return;
        coolDownTime = SkillData.CoolDownTime;

        Buff buff = BuffManager.instance.GetBuff("毒药");
        if (buff != null)
        {
            Debug.Log("获得实例成功");
        }
        buff.ReturnBuffDataInfo((info) =>
        {
            Debug.Log("毒药的描述是"+info.description);
        });

        GameObject player = GameObject.FindAnyObjectByType<Player>().gameObject;
        buff.Apply(player);
        player.GetComponent<BuffHandle>().AddBuff(buff);
    }
}
