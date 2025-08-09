using System;
using UnityEngine;

namespace GorillaPad.Logic.UI
{
    public class GorillaPadButton : GorillaPressableButton
    {
        private Action OnButtonPress;
        public static GorillaPadButton Create(GameObject GObj, Action CodeToRun)
        {
            GorillaPadButton BScript = GObj.GetComponent<GorillaPadButton>();
            if (BScript == null)
                BScript = GObj.AddComponent<GorillaPadButton>();

            GObj.layer = 18;

            BoxCollider BCol = GObj.GetComponent<BoxCollider>();
            if (BCol == null)
                BCol = GObj.AddComponent<BoxCollider>();

            BCol.isTrigger = true;

            BScript.OnButtonPress = CodeToRun;
            return BScript;
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();
            OnButtonPress?.Invoke();
        }
    }
}
