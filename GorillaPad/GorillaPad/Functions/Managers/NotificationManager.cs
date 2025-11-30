using System;
using System.Collections.Generic;
using System.Text;
using GorillaPad.Functions.UI;
using GorillaPad.Tools;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

namespace GorillaPad.Functions.Managers
{
    public class NotificationManager : MonoBehaviourPunCallbacks
    {
        private NotifAudio Audio;
        public static void SendNotification(Text Title, Text Message, NotifAudio Audio)
        {
            /* Set a max cap to the title and if over put "..." same with message, full message and title displayed in inbox app.
               Set TitleText and Message Text.
               Play Animation & play a sound.
               Send All Information To InboxApp */

            AudioSource BuzzAudio = null;
            AudioSource PowerAudioNull = null;
            AudioSource ButtonAudioNull = null;
            ContentLoader.GetSounds(ref PowerAudioNull, ref ButtonAudioNull, ref BuzzAudio);

            if (Audio == NotifAudio.BuzzAudio)
                BuzzAudio.Play();
        }

        public enum NotifAudio
        {
            BuzzAudio
        }
    }
}
