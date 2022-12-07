using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace kaleido
{
    public class FallingManager : MonoBehaviour
    {
        public static Action<TurnState> OnSideTurn;

        [SerializeField] private RectTransform fallingRegion;

        [SerializeField] private RectTransform leftSpawnPos;
        [SerializeField] private RectTransform leftTargetPos;
        [SerializeField] private RectTransform RightSpawnPos;
        [SerializeField] private RectTransform RightTargetPos;

        [SerializeField] private RectTransform leftFeetObj;
        [SerializeField] private RectTransform rightFeetObj;

        [SerializeField] private int leftFootCount = 10;
        [SerializeField] private int rightFootCount = 10;

        [SerializeField] private float spawnInterval = 0.8f;
        [SerializeField] private float reachScnds = 1.5f;

        private List<RectTransform> m_leftFootObjs;
        private List<RectTransform> m_rightFootObjs;

        private int m_leftIndex = 0;
        private int m_rightIndex = 0;

        private Coroutine m_status;
        private bool m_isleftPressed = false;
        private bool m_isRightPressed = false;

        private void Awake()
        {
            m_leftFootObjs = new List<RectTransform>();
            m_rightFootObjs = new List<RectTransform>();

            for (int i = 0; i < leftFootCount; i++)
            {
                var it = Instantiate(leftFeetObj, fallingRegion);
                it.gameObject.SetActive(false);
                m_leftFootObjs.Add(it);
            }
            for (int i = 0; i < rightFootCount; i++)
            {
                var it = Instantiate(rightFeetObj, fallingRegion);
                it.gameObject.SetActive(false);
                m_rightFootObjs.Add(it);
            }
        }
        private void OnEnable()
        {
            PlayerController.OnPlayerFell += onPlayerFell;
            InputManager.RightFootUIPressed += onRightUIPressed;
            InputManager.LeftFootUIPressed += onLeftUIPressed;
        }
        private void OnDisable()
        {
            PlayerController.OnPlayerFell -= onPlayerFell;
            InputManager.RightFootUIPressed -= onRightUIPressed;
            InputManager.LeftFootUIPressed -= onLeftUIPressed;
        }
        private void onLeftUIPressed()
        {
            m_isleftPressed = true;
        }
        private void onRightUIPressed()
        {
            m_isRightPressed = true;
        }
        private void onPlayerFell()
        {
            m_isleftPressed = false;
            m_isRightPressed = false;

            if (m_status != null)
            {
                StopCoroutine(m_status);
            }
            foreach (var item in m_leftFootObjs)
            {
                LeanTween.cancel(item.gameObject);
                item.gameObject.SetActive(false);
            }
            foreach (var item in m_rightFootObjs)
            {
                LeanTween.cancel(item.gameObject);
                item.gameObject.SetActive(false);
            }
        }
        private void Start()
        {
            m_status = StartCoroutine(spawn());
        }

        IEnumerator spawn()
        {
            while (true)
            {
                float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                if(rand<= 0.5)
                {
                    m_leftFootObjs[m_leftIndex].gameObject.SetActive(true);
                    m_leftFootObjs[m_leftIndex].transform
                        .localPosition 
                        = leftSpawnPos.localPosition;
                    int index = m_leftIndex;
                    bool firsTime = true;
                   
                    LeanTween.moveLocal(m_leftFootObjs[m_leftIndex].gameObject,
                        leftTargetPos.localPosition,
                        reachScnds)
                        .setOnUpdate((Vector3 val)=> {
                            if(firsTime && 
                            Vector3.Distance(val, leftTargetPos.localPosition) 
                                < m_leftFootObjs[m_leftIndex].rect.height)
                            {
                                OnSideTurn?.Invoke(TurnState.LEFT);
                                firsTime = false;

                            }
                            if(m_isleftPressed && Vector3.Distance(val, leftTargetPos.localPosition)
                                < m_leftFootObjs[m_leftIndex].rect.height)
                            {
                                m_leftFootObjs[index].gameObject.SetActive(false);
                                m_isleftPressed = false;
                            }
                        })
                        .setOnComplete(()=> {
                            m_leftFootObjs[index].gameObject.SetActive(false);
                        });
                    

                    m_leftIndex = (m_leftIndex + 1) % leftFootCount;
                }
                else
                {

                    m_rightFootObjs[m_rightIndex].gameObject.SetActive(true);
                    m_rightFootObjs[m_rightIndex].transform
                        .localPosition
                        = RightSpawnPos.localPosition;
                    int index = m_rightIndex;
                    bool firsTime = true;
                    
                    LeanTween.moveLocal(m_rightFootObjs[m_rightIndex].gameObject,
                        RightTargetPos.localPosition,
                        reachScnds)
                        .setOnUpdate((Vector3 val)=> {

                            if (firsTime && Vector3.Distance(val, RightTargetPos.localPosition) 
                                < m_rightFootObjs[m_rightIndex].rect.height)
                            {
                                OnSideTurn?.Invoke(TurnState.RIGHT);
                                firsTime = false;
                            }
                            if(m_isRightPressed && Vector3.Distance(val, RightTargetPos.localPosition)
                                < m_rightFootObjs[m_rightIndex].rect.height)
                            {
                                m_rightFootObjs[index].gameObject.SetActive(false);
                                m_isRightPressed = false;
                            }

                        })
                        .setOnComplete(() => {
                            m_rightFootObjs[index].gameObject.SetActive(false);
                        });
                    m_rightIndex = (m_rightIndex + 1) % rightFootCount;
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }

    }
}