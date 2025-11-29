using UnityEngine;

namespace GorillaPad.Functions.Managers
{
    public class AnimationManager : MonoBehaviour
    {
        private GameObject objOne;
        private GameObject objTwo;

        private CanvasGroup objOneCanvasGroup;
        private CanvasGroup objTwoCanvasGroup;

        private bool isTransitioning = false;
        private bool transitioningToTwo = false;
        private float transitionStartTime = 0f;
        private float transitionDuration = 0.25f;

        public static AnimationManager CreateAnimation(GameObject from, GameObject to, bool toSecond)
        {
            if (from == null) return null;

            var manager = from.AddComponent<AnimationManager>();
            manager.objOne = from;
            manager.objTwo = to;

            manager.objOneCanvasGroup = manager.GetOrAddCanvasGroup(from);
            if (to != null)
                manager.objTwoCanvasGroup = manager.GetOrAddCanvasGroup(to);

            from.SetActive(true);
            if (to != null) to.SetActive(true);

            manager.StartTransition(toSecond);

            return manager;
        }

        private void StartTransition(bool toSecond)
        {
            isTransitioning = true;
            transitionStartTime = Time.time;
            transitioningToTwo = toSecond;

            if (objTwo == null)
            {
                objOneCanvasGroup.alpha = toSecond ? 0f : 1f;
                objOne.transform.localScale = toSecond ? Vector3.zero : Vector3.one;
                objOne.SetActive(true);
            }
            else
            {
                objOneCanvasGroup.alpha = toSecond ? 1f : 0f;
                objTwoCanvasGroup.alpha = toSecond ? 0f : 1f;
                objOne.SetActive(true);
                objTwo.SetActive(true);
            }

            objOneCanvasGroup.interactable = false;
            objOneCanvasGroup.blocksRaycasts = false;

            if (objTwoCanvasGroup != null)
            {
                objTwoCanvasGroup.interactable = false;
                objTwoCanvasGroup.blocksRaycasts = false;
            }
        }

        private void UpdateTransition()
        {
            if (!isTransitioning) return;

            float elapsed = Time.time - transitionStartTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            float eased = 1f - Mathf.Pow(1f - t, 2f);

            if (objTwo == null)
            {
                objOneCanvasGroup.alpha = transitioningToTwo ? Mathf.Lerp(0f, 1f, eased) : Mathf.Lerp(1f, 0f, eased);
                objOne.transform.localScale = transitioningToTwo ? Vector3.Lerp(Vector3.zero, Vector3.one, eased) : Vector3.Lerp(Vector3.one, Vector3.zero, eased);
            }
            else
            {
                objOneCanvasGroup.alpha = transitioningToTwo ? Mathf.Lerp(1f, 0f, eased) : Mathf.Lerp(0f, 1f, eased);
                objTwoCanvasGroup.alpha = transitioningToTwo ? Mathf.Lerp(0f, 1f, eased) : Mathf.Lerp(1f, 0f, eased);
            }

            if (t >= 1f)
            {
                isTransitioning = false;

                if (objTwo == null)
                {
                    objOneCanvasGroup.alpha = transitioningToTwo ? 1f : 0f;
                    objOne.transform.localScale = Vector3.one;
                    objOne.SetActive(transitioningToTwo);
                    ScreenManager.CurrentScreen = objOne;
                }
                else
                {
                    objOne.SetActive(!transitioningToTwo);
                    objTwo.SetActive(transitioningToTwo);

                    objOneCanvasGroup.alpha = 1f;
                    objOneCanvasGroup.interactable = true;
                    objOneCanvasGroup.blocksRaycasts = true;

                    objTwoCanvasGroup.alpha = 1f;
                    objTwoCanvasGroup.interactable = true;
                    objTwoCanvasGroup.blocksRaycasts = true;

                    ScreenManager.CurrentScreen = transitioningToTwo ? objTwo : objOne;
                }
            }
        }

        private void Update()
        {
            if (isTransitioning)
                UpdateTransition();
        }

        private CanvasGroup GetOrAddCanvasGroup(GameObject obj)
        {
            var cg = obj.GetComponent<CanvasGroup>();
            if (cg == null) cg = obj.AddComponent<CanvasGroup>();
            return cg;
        }
    }
}
