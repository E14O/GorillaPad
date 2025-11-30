using System.Collections;
using GorillaPad.Tools;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaPad.Functions.Managers
{
    public class NotificationManager : MonoBehaviourPunCallbacks
    {
        private NotifAudio Audio;
        public static GameObject NotificationLockScreen, LockScreen;
        private int NotifAmountLockScreen = 0;

        void Start()
        {
            NotificationLockScreen = ContentLoader.BundleParent.transform.GetChild(1).GetChild(5).transform.Find("Notification").gameObject;
            LockScreen = ContentLoader.BundleParent.transform.GetChild(1).GetChild(5).gameObject;
            NotificationLockScreen.SetActive(false);
        }

        public static void SendNotification(string Title, string Message)
        {
            /* Set a max cap to the title and if over put "..." same with message, full message and title displayed in inbox app.
               Set TitleText and Message Text.
               Play Animation & play a sound.
               Send All Information To InboxApp */

            

            var NewNotif = Instantiate(NotificationLockScreen, NotificationLockScreen.transform.parent);
            NewNotif.transform.SetParent(LockScreen.transform, false);
            NewNotif.transform.GetChild(0).GetComponent<Text>().text = Title;
            NewNotif.transform.GetChild(1).GetComponent<Text>().text = Message;

            NewNotif.transform.GetChild(2).GetComponent<Text>().text = $"{System.DateTime.Now}";
            NewNotif.SetActive(true);



            /* AudioSource BuzzAudio = null;
            AudioSource PowerAudioNull = null;
            AudioSource ButtonAudioNull = null;
            ContentLoader.GetSounds(ref PowerAudioNull, ref ButtonAudioNull, ref BuzzAudio);

            if (Audio == NotifAudio.BuzzAudio)
                BuzzAudio.Play(); */
        }

        public enum NotifAudio
        {
            BuzzAudio
        }
    }
}
