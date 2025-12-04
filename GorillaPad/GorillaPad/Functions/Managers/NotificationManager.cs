using System.Collections;
using System.Collections.Generic;
using GorillaPad.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaPad.Functions.Managers
{
    public class NotificationManager : MonoBehaviour
    {
        public static GameObject TemporaryNotification { get; private set; }
        public static GameObject LockScreenGrid { get; private set; }

        private int NotificationCount = 0;

        void Start()
        {
            TemporaryNotification = ContentLoader.BundleParent.transform.Find("Canvas/LockScreen/Grid/Notification").gameObject;
            LockScreenGrid = ContentLoader.BundleParent.transform.Find("Canvas/LockScreen/Grid").gameObject;

            TemporaryNotification.SetActive(false);
        }

        public static void SendNotification(string Title, string Message)
        {
            var Notification = Instantiate(TemporaryNotification, LockScreenGrid.transform, false);
            Notification.transform.SetSiblingIndex(0);

            Notification.transform.GetChild(0).GetComponent<Text>().text = Title;
            Notification.transform.GetChild(1).GetComponent<Text>().text = Message;

            Notification.transform.GetChild(2).GetComponent<Text>().text = $"{System.DateTime.Now}";
            Notification.SetActive(true);

            PadLogging.LogInfo($"Received Notification: {Title}, {Message}");

            /* AudioSource BuzzAudio = null;
               AudioSource PowerAudioNull = null;
               AudioSource ButtonAudioNull = null;

               ContentLoader.GetSounds(ref PowerAudioNull, ref ButtonAudioNull, ref BuzzAudio);
               BuzzAudio.Play();

             /* AudioSource BuzzAudio = null;
             AudioSource PowerAudioNull = null;
             AudioSource ButtonAudioNull = null;
             ContentLoader.GetSounds(ref PowerAudioNull, ref ButtonAudioNull, ref BuzzAudio);

             if (Audio == NotifAudio.BuzzAudio)
                 BuzzAudio.Play(); */
        }
    }
}
