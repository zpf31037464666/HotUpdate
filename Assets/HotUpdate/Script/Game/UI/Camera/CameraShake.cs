using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : Singleton<CameraShake>
{
    public CinemachineVirtualCamera virtualCamera; // 引用虚拟相机

    private CinemachineBasicMultiChannelPerlin perlin; // 用于控制震动的组件

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

    public void ShakeCamera(float shakeDuration, float shakeAmplitude, float shakeFrequency)
    {
        if (shakeCoroutine!=null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine= StartCoroutine(Shake(shakeDuration, shakeAmplitude, shakeFrequency));
    }

    private IEnumerator Shake(float shakeDuration, float shakeAmplitude, float shakeFrequency)
    {
        perlin.m_AmplitudeGain = shakeAmplitude; // 设置震动幅度
        perlin.m_FrequencyGain = shakeFrequency; // 设置震动频率

        yield return new WaitForSeconds(shakeDuration); // 等待震动持续时间

        perlin.m_AmplitudeGain = 0; // 震动结束，幅度归零
    }
}
