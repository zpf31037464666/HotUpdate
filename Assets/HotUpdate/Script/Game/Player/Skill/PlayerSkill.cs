using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public List<Skill> SkillList= new List<Skill>();


    public static event Action<Skill> OnSkillAddEvent;
    public static event Action<Skill> OnSkillRemoveEvent;
    public static event Action<List<Skill>> OnSkillUpdateEvent;

    public void AddSkill(Skill skill)
    {
        if(!SkillList.Contains(skill))
        {
            SkillList.Add(skill);
            OnSkillAddEvent?.Invoke(skill);
        }
    }
    public void ReMoveSkill(Skill skill)
    {
        if (SkillList.Contains(skill))
        {
            SkillList.Remove(skill);
            OnSkillRemoveEvent?.Invoke(skill);
        }
    }

    private void Update()
    {
        foreach(Skill skill in SkillList)
        {
            skill.coolDownTime-= Time.deltaTime;
            if(skill.coolDownTime <= 0)
            {
                skill.coolDownTime = 0;
            }
        }
        OnSkillUpdateEvent?.Invoke(SkillList);



        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("增加技能");
            Skill skill = SkillManager.instance.GetSkill("加血技能");
            skill.ReturnSkillDataInfo((info) => { });
            AddSkill(skill);

            Debug.Log("增加技能");
            Skill skill2 = SkillManager.instance.GetSkill("扣血技能");
            skill2.ReturnSkillDataInfo((info) => { });
            AddSkill(skill2
                );

        }
    }

}
