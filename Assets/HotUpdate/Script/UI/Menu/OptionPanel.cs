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
            // UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);
             UIManager.Instance.ReturnToPreviousPanel();
        });
        backButton.onClick.AddListener(() =>
        {
            //UIManager.Instance.SwitchPanel(My_UIConst.MainMenuPanel);
            UIManager.Instance.ReturnToPreviousPanel();
        });
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale =0;
        canvas.sortingOrder=99;
        musicAudioSlider.value= AudioManager.instance.GetMusicVolume;
        sfxAudioSlider.value= AudioManager.instance.GetSfxVolume;
    }
    public override void Exit()
    {
        base.Exit();
        Time.timeScale =1;
        canvas.sortingOrder=0;
    }

}
