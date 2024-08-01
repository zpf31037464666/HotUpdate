using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : UIState
{
    [SerializeField] private Slider musicAudioSlider;
    [SerializeField] private Slider sfxAudioSlider;

    [SerializeField] private Button comfirmButton;
    [SerializeField] private Button backButton;

    private void Start()
    {
        musicAudioSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.instance.MusicPlayerVolume(musicAudioSlider.value);
        });
        sfxAudioSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.instance.SFXPlayerVolume(sfxAudioSlider.value);
        });

        comfirmButton.onClick.AddListener(() =>
        {
             UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);
        });
        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);
        });
    }

    public override void Enter()
    {
        base.Enter();
        musicAudioSlider.value= AudioManager.instance.GetMusicVolume;
        sfxAudioSlider.value= AudioManager.instance.GetSfxVolume;

    }

}
