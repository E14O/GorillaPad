using System;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad.Logic.UI
{
    public class PadButton : GorillaPressableButton
    {
        private Action OnButtonPress;
        public static PadButton Create(GameObject Object, Action ExecuteFunction)
        {
            PadButton PBScript = Object.GetComponent<PadButton>() ?? Object.AddComponent<PadButton>();
            BoxCollider ObjectCollider = Object.GetComponent<BoxCollider>() ?? Object.AddComponent<BoxCollider>();

            ObjectCollider.isTrigger = true;
            Object.layer = 18;

            PBScript.OnButtonPress = ExecuteFunction;
            return PBScript;
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();
            pressButtonSoundIndex = 0;

            // do what you can with this 
            AudioSource powerAudio = null;
            AudioSource buttonAudio = null;
            ContentLoader.GetSounds(ref powerAudio, ref buttonAudio);
            buttonAudio.Play();

            OnButtonPress?.Invoke();
        }

    }
}
