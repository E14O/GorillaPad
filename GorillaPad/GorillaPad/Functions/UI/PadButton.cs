using System;
using GorillaPad.Tools;
using Photon.Pun;
using UnityEngine;

namespace GorillaPad.Functions.UI
{
    public class PadButton : GorillaPressableButton
    {
        private Action OnButtonPress;
        private SelectedAudio selectedSound;

        public static PadButton Create(Transform Parent, string ObjectName, SelectedAudio SoundType, Action ExecuteFunction)
        {
            GameObject Obj = Parent.Find(ObjectName).gameObject;
            if (Obj == null)
            {
                PadLogging.LogError($"Unable to find [{ObjectName}] in [{Parent}]");
                return null;
            }

            PadButton PbScript = Obj.GetComponent<PadButton>() ?? Obj.AddComponent<PadButton>();
            BoxCollider ObjCollider = Obj.GetComponent<BoxCollider>() ?? Obj.AddComponent<BoxCollider>();
            ObjCollider.isTrigger = true;

            Obj.layer = 18;

            PbScript.OnButtonPress = ExecuteFunction;
            PbScript.selectedSound = SoundType;

            return PbScript;
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();

            AudioSource PowerAudio = null;
            AudioSource ButtonAudio = null;
            AudioSource BuzzAudio = null;

            ContentLoader.GetSounds(ref PowerAudio, ref ButtonAudio, ref BuzzAudio);

            if (selectedSound == SelectedAudio.PowerAudio)
                PowerAudio.Play();
            else if (selectedSound == SelectedAudio.ButtonAudio)
                ButtonAudio.Play();

            pressButtonSoundIndex = 0;
            OnButtonPress.Invoke();
        }
    }

    public enum SelectedAudio
    {
        PowerAudio,
        ButtonAudio
    }
}