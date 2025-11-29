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

        public static PadButton Create(Transform parent, string objectName, SelectedAudio soundType, Action executeFunction)
        {

            GameObject obj = parent.Find(objectName).gameObject;
            if (obj == null)
            {
                PadLogging.LogError("Could Not Find");
                return null;
            }

            PadButton pbScript = obj.GetComponent<PadButton>() ?? obj.AddComponent<PadButton>();
            BoxCollider objCollider = obj.GetComponent<BoxCollider>() ?? obj.AddComponent<BoxCollider>();
            objCollider.isTrigger = true;

            obj.layer = 18;

            pbScript.OnButtonPress = executeFunction;
            pbScript.selectedSound = soundType;
            return pbScript;
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();

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