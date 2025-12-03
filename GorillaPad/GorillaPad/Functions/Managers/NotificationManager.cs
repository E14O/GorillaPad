<<<<<<< HEAD
﻿using GorillaPad.Tools;
=======
﻿using System.Collections;
using System.Collections.Generic;
using GorillaPad.Tools;
>>>>>>> d103f4148e0d4d982d65683d411666112ee83846
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using GFriends = GorillaFriends.Main;

namespace GorillaPad.Functions.Managers
{
    public class NotificationManager : MonoBehaviourPunCallbacks
    {
<<<<<<< HEAD
        public static GameObject TemporaryNotification { get; private set; }
        public static GameObject LockScreenGrid { get; private set; }

        private int NotificationCount = 0;

        void Start()
        {
            TemporaryNotification = ContentLoader.BundleParent.transform.Find("Canvas/LockScreen/Grid/Notification").gameObject;
            LockScreenGrid = ContentLoader.BundleParent.transform.Find("Canvas/LockScreen/Grid").gameObject;

            TemporaryNotification.SetActive(false);
=======
        private NotifAudio Audio;
        public static GameObject NotificationLockScreen, LockScreen;
        private int NotifAmountLockScreen = 0;
        private static Dictionary<GameObject, float> NotificationTimes = new Dictionary<GameObject, float>();
        private static NotificationManager instance;

        void Start()
        {
            NotificationLockScreen = ContentLoader.BundleParent.transform.Find("Canvas/LockScreen/Grid/Notification").gameObject;
            LockScreen = ContentLoader.BundleParent.transform.Find("Canvas/LockScreen/Grid").gameObject;
            NotificationLockScreen.SetActive(false);
            instance = this;
            StartCoroutine(UpdateNotificationTimes());
        }

        private IEnumerator UpdateNotificationTimes()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                UpdateAllNotificationTimes();
            }
        }

        private static void UpdateAllNotificationTimes()
        {
            var notificationsToRemove = new List<GameObject>();

            foreach (var kvp in NotificationTimes)
            {
                if (kvp.Key == null)
                {
                    notificationsToRemove.Add(kvp.Key);
                    continue;
                }

                float elapsedSeconds = Time.time - kvp.Value;
                int seconds = Mathf.FloorToInt(elapsedSeconds);
                

                Text timeText = kvp.Key.transform.GetChild(2).GetComponent<Text>();
                if (timeText != null)
                {
                    timeText.text = $"{seconds}s ago";
                   
                }
            }

            foreach (var notif in notificationsToRemove)
            {
                NotificationTimes.Remove(notif);
            }
        }

        public override void OnJoinedRoom()
        {
            SendNotification("RoomInfo", "You have joined a room");
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            bool isFriend = GFriends.IsFriend(newPlayer.UserId);
            string message = isFriend 
                ? $"your friend \"{newPlayer.NickName}\" has joined the room"
                : $"{newPlayer.NickName} has joined the room";
            SendNotification("RoomInfo", message);
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            
            SendNotification("RoomInfo", $"{otherPlayer.NickName} has left the room");
<<<<<<< Updated upstream
=======
>>>>>>> d103f4148e0d4d982d65683d411666112ee83846
>>>>>>> Stashed changes
        }

        public static void SendNotification(string Title, string Message)
        {
            var Notification = Instantiate(TemporaryNotification, LockScreenGrid.transform, false);
            Notification.transform.SetSiblingIndex(0);

<<<<<<< Updated upstream

=======
<<<<<<< HEAD
            Notification.transform.GetChild(0).GetComponent<Text>().text = Title;
            Notification.transform.GetChild(1).GetComponent<Text>().text = Message;
=======
>>>>>>> Stashed changes

>>>>>>> d103f4148e0d4d982d65683d411666112ee83846

<<<<<<< Updated upstream
=======
            Notification.transform.GetChild(2).GetComponent<Text>().text = $"{System.DateTime.Now}";
            Notification.SetActive(true);

<<<<<<< HEAD
            PadLogging.LogInfo($"Received Notification: {Title}, {Message}");
=======
>>>>>>> Stashed changes
            float creationTime = Time.time;
            NotificationTimes[NewNotif] = creationTime;
            NewNotif.transform.GetChild(2).GetComponent<Text>().text = "0s ago";
            NewNotif.SetActive(true);
>>>>>>> d103f4148e0d4d982d65683d411666112ee83846

           /* AudioSource BuzzAudio = null;
              AudioSource PowerAudioNull = null;
              AudioSource ButtonAudioNull = null;

              ContentLoader.GetSounds(ref PowerAudioNull, ref ButtonAudioNull, ref BuzzAudio);
              BuzzAudio.Play();

              Set a max cap to the title and if over put "..." same with message, full message and title displayed in inbox app.
              Set TitleText and Message Text.
              Play Animation & play a sound.
              Send All Information To InboxApp */
        }
    }
}
