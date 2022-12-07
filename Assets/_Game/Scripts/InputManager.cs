using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace kaleido
{
    public class InputManager : MonoBehaviour
    {
        public static Action LeftFootUIPressed;
        public static Action RightFootUIPressed;

        [SerializeField] private GameObject leftFootUI;
        [SerializeField] private GameObject rightFootUI;

        public void LeftFootUIDown()
        {
            LeanTween.scale(leftFootUI, Vector3.one * 1.1f, 0.08f);
        }
        public void LeftFootUIUp()
        {
            LeanTween.scale(leftFootUI, Vector3.one, 0.08f);
            LeftFootUIPressed?.Invoke();

        }
        public void RightFootUIDown()
        {
            LeanTween.scale(rightFootUI, Vector3.one * 1.1f, 0.08f);

        }
        public void RightFootUIUp()
        {
            LeanTween.scale(rightFootUI, Vector3.one, 0.08f);
            RightFootUIPressed?.Invoke();
        }

    }

}