using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace kaleido
{
    public enum TurnState
    {
        LEFT,
        RIGHT,
        NONE
    }
    public class PlayerController : MonoBehaviour
    {
        public static Action OnPlayerFell;

        private TurnState m_turnState=TurnState.NONE;

        private Animator m_animatorController;
        private bool m_isFell = false;
        private bool m_isActivate = false;

        private void Awake()
        {
            m_animatorController = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            InputManager.RightFootUIPressed += onRightUIPressed;
            InputManager.LeftFootUIPressed += onLeftUIPressed;
            FallingManager.OnSideTurn += onSideTurn;
        }
        private void OnDisable()
        {
            InputManager.RightFootUIPressed -= onRightUIPressed;
            InputManager.LeftFootUIPressed -= onLeftUIPressed;
            FallingManager.OnSideTurn -= onSideTurn;

        }
        private void onSideTurn(TurnState turnState)
        {
            if (m_isFell)
                return;

            if (m_isActivate == true)
            {
                m_animatorController.SetTrigger("Fall");
                OnPlayerFell?.Invoke();
                m_isFell = true;
            }
            m_turnState = turnState;
            m_isActivate = true;
        }
        private void onLeftUIPressed()
        {
            if (m_isFell)
                return;

            if (m_turnState != TurnState.LEFT)
            {
                m_animatorController.SetTrigger("Fall");
                OnPlayerFell?.Invoke();
                m_isFell = true;
            }
            else if(m_turnState == TurnState.LEFT)
            {
                m_turnState = TurnState.NONE;
                m_isActivate = false;

            }
        }
        private void onRightUIPressed()
        {
            if (m_isFell)
                return;

            if(m_turnState != TurnState.RIGHT)
            {
                m_animatorController.SetTrigger("Fall");
                m_isFell = true;

                OnPlayerFell?.Invoke();

            }
            else if(m_turnState == TurnState.RIGHT)
            {
                m_turnState = TurnState.NONE;
                m_isActivate = false;
            }

        }

    }
}

