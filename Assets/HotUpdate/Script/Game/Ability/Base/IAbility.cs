using System;

public interface IAbility
{
    string Name { get; }

    AbilityData data { get; }

    void ReturnTaskInfo(Action<AbilityInfo> callback);
    void Activate(Player player);
}
