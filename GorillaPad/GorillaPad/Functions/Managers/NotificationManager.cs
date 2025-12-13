using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GorillaPad.Tools;

namespace GorillaPad.Functions.Managers
{
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance;

        public static GameObject NotificationLockScreen;
        public static GameObject NotificationOtherScreen;
        public static GameObject LockScreenGrid;
        public static GameObject HomeScreenGrid;

        private bool NotificationActive = false;

        private const int MaxLockScreenNotifications = 4;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            NotificationLockScreen = ContentLoader.BundleParent.transform.Find("Canvas/LockScreen/Grid/Notification").gameObject;

            LockScreenGrid = ContentLoader.BundleParent.transform.Find("Canvas/LockScreen/Grid").gameObject;

            NotificationOtherScreen =ContentLoader.BundleParent.transform.Find("Canvas/HomeScreen/Notification").gameObject;

            HomeScreenGrid = ContentLoader.BundleParent.transform.Find("Canvas/HomeScreen").gameObject;

            NotificationLockScreen.SetActive(false);
            NotificationOtherScreen.SetActive(false);
        }

        public void SendNotification(string title, string message)
        {
            PadLogging.LogInfo($"Received Notification: {title}, {message}");

            // Send Data To Inbox App Then Display There

            StartCoroutine(CreateLockScreenNotification(title, message));
            StartCoroutine(CreateHomeScreenNotification(title, message));
        }

        private IEnumerator CreateLockScreenNotification(string title, string message)
        {

            GameObject Notification = Instantiate(NotificationLockScreen, LockScreenGrid.transform, false);
            Notification.transform.SetSiblingIndex(0);

            Notification.transform.GetChild(0).GetComponent<Text>().text = title;
            Notification.transform.GetChild(1).GetComponent<Text>().text = message;
            Notification.transform.GetChild(2).GetComponent<Text>().text = System.DateTime.Now.ToString("HH:mm");

            int CurrentNotifications = 0;
            for (int i = 0; i < LockScreenGrid.transform.childCount; i++)
            {
                if (LockScreenGrid.transform.GetChild(i).gameObject.activeSelf)
                    CurrentNotifications++;
            }

            Notification.SetActive(CurrentNotifications < MaxLockScreenNotifications);

            yield return new WaitForSeconds(20f);

            if (Notification != null)
                Destroy(Notification);
        }

        private IEnumerator CreateHomeScreenNotification(string title, string message)
        {
            while (NotificationActive)
                yield return null;

            NotificationActive = true;

            yield return new WaitForSeconds(2f);

            GameObject notification = Instantiate(NotificationOtherScreen,HomeScreenGrid.transform, false);

            notification.transform.Find("Header").GetComponent<Text>().text = title;
            notification.transform.Find("Text").GetComponent<Text>().text = message;
            notification.transform.Find("TimeSent").GetComponent<Text>().text = System.DateTime.Now.ToString("HH:mm");

            notification.SetActive(true);

            Transform NotificationPosition = notification.transform;

            Vector3 FromPos = new Vector3(-1.114f, -0.021f, 0.003f);
            Vector3 ToPos = new Vector3(-0.814f, -0.021f, 0.003f);

            float MoveSpeed = 1.5f;
            float TransformLerp = 0f;

            while (TransformLerp < 1f)
            {
                TransformLerp += Time.deltaTime * MoveSpeed;
                NotificationPosition.localPosition = Vector3.Lerp(FromPos, ToPos, TransformLerp);
                yield return null;
            }

            yield return new WaitForSeconds(3f);

            TransformLerp = 0f;

            while (TransformLerp < 1f)
            {
                TransformLerp += Time.deltaTime * MoveSpeed;
                NotificationPosition.localPosition = Vector3.Lerp(ToPos, FromPos, TransformLerp);
                yield return null;
            }

            Destroy(notification);

            NotificationActive = false;
        }

    }
}
