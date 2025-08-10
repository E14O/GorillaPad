using System;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad.Functions.UI
{

    public class PadButton : GorillaPressableButton
    {
        private Action OnButtonPress;

        private SelectedAudio selectedSound;

        public static PadButton Create(GameObject Object, SelectedAudio soundType, Action ExecuteFunction)
        {
            PadButton PBScript = Object.GetComponent<PadButton>() ?? Object.AddComponent<PadButton>();
            BoxCollider ObjectCollider = Object.GetComponent<BoxCollider>() ?? Object.AddComponent<BoxCollider>();

            ObjectCollider.isTrigger = true;
            Object.layer = 18;

            PBScript.OnButtonPress = ExecuteFunction;
            PBScript.selectedSound = soundType;
            return PBScript;
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();
            pressButtonSoundIndex = 0;

            AudioSource powerAudio = null;
            AudioSource buttonAudio = null;
            ContentLoader.GetSounds(ref powerAudio, ref buttonAudio);

            if (selectedSound == SelectedAudio.PowerAudio)
                powerAudio.Play();
            else if (selectedSound == SelectedAudio.ButtonAudio)
                buttonAudio.Play();

            OnButtonPress.Invoke();
        }
    }

    public enum SelectedAudio
    {
        PowerAudio,
        ButtonAudio
    }
}

