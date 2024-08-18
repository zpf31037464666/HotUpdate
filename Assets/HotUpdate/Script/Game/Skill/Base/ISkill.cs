using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    string Name { get; }
    SkillData SkillData { get; set; }

    void Apply();

    void ReturnSkillDataInfo(Action<SkillInfo> callback);
}
