using UnityEngine;
using Cinemachine;
using System.Collections;
using TreeEditor;

public class CameraShake : Singleton<CameraShake>
{
    public CinemachineVirtualCamera virtualCamera; // 引用虚拟相机

    private CinemachineBasicMultiChannelPerlin perlin; // 用于控制震动的组件

    [SerializeField] private float gloablShakeForce = 1f;


    Coroutine shakeCoroutine;

    void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Virtual Camera is not assigned! Please assign it in the Inspector.");
            return; // 退出 Start 方法，避免后续代码执行
        }

        // 获取或添加 CinemachineBasicMultiChannelPerlin 组件
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            perlin = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        perlin.m_AmplitudeGain = 0; // 初始化震动幅度为0
    }

    public void SetFollowPlayer(Transform player)
    {
        virtualCamera.Follow = player; // 设置相机跟随玩家
        virtualCamera.LookAt = player; // 设置相机朝向玩家
    }

    public void CamerShake(float shakeDuration, float shakeAmplitude, float shakeFrequency)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake(shakeDuration, shakeAmplitude, shakeFrequency));
    }

    public void CamerShake(CinemachineImpulseSource shakeSource)
    {
        shakeSource.GenerateImpulseWithForce(gloablShakeForce);

    }
    private IEnumerator Shake(float shakeDuration, float shakeAmplitude, float shakeFrequency)
    {
        perlin.m_AmplitudeGain = shakeAmplitude; // 设置震动幅度
        perlin.m_FrequencyGain = shakeFrequency; // 设置震动频率

        // 在震动期间保持初始旋转
        Quaternion originalRotation = Quaternion.identity;

        // 等待震动持续时间
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            // 每帧保持相机的旋转为初始旋转
           transform.rotation = originalRotation;
            elapsed += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        perlin.m_AmplitudeGain = 0; // 震动结束，幅度归零
        perlin.m_FrequencyGain= 0;

        // 恢复摄像机的初始位置和旋转
      transform.rotation = originalRotation;
    }
}
