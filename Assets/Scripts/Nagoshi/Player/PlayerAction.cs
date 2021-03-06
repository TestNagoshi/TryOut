﻿//////////////////////
//製作者　名越大樹
//製作日　10月2日
//クラス名　プレイヤーの操作に関するクラス
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagoshi
{
    public class PlayerAction : MonoBehaviour
    {
        [SerializeField]
        GameObject seManager;
        [SerializeField]
        PlayerAnimation playerAnimationScript;
        [SerializeField]
        PlayerStatus playerStatusScript;
        int moveValue = 0;
        [SerializeField]
        float leftValue;
        [SerializeField]
        float rightValue;

        bool isEffect = false;
        float cnt = 0.0f;

        void Start()
        {
            
        }

        void Update()
        {
            
            if (playerStatusScript.GetIsWalk())
            {
                Move();
                
            }

            if (!isEffect)
            {
                Key();
            }
            else
            {
                EffectAction();
            }
        }

        void Key()
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal") < leftValue)
            {
                Walk(true, -1);
            }

            else if (Input.GetKeyUp(KeyCode.D) || Input.GetAxis("Horizontal") == 0)
            {
                Walk(false, 0);
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal") > rightValue)
            {
                Walk(true, 1);
            }

            else if (Input.GetKeyUp(KeyCode.A) || Input.GetAxis("Horizontal") == 0)
            {
                Walk(false, 0);
            }

            if(Input.GetAxis("Vertical") != 0.0f)
            {
                ElevatorAction();
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick2Button1)　|| Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                Event();
            }

            if(Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0))
            {
                Jump();
            }
        }

        /// <summary>
        /// 移動キーを押された時の処理
        /// </summary>
        void Walk(bool set, int value)
        {
            //アニメーションの処理開始
            playerStatusScript.SetIsWalk(set);
            playerAnimationScript.SetIsWalk();
            //アニメーションの処理終了
            Vector3 pos = transform.localScale;
            moveValue = value;
            if (value != 0)
            {
                pos.z = pos.z * value;
                transform.localScale = pos;
            }
        }

        void Move()
        {
            if (moveValue != 0)
            {
                float speed = playerStatusScript.GetSpeed() * Time.deltaTime * moveValue;
                transform.Translate(speed, 0.0f, 0.0f, Space.World);
            }
        }

        void Event()
        {
            Nagoshi.PlayerStatus.EventStatus result = playerStatusScript.GetEventStatus();
            if (result != PlayerStatus.EventStatus.none)
            {
                GameObject eventobj = playerStatusScript.GetEventObj();
                int money = playerStatusScript.GetMoney();
                int rate = eventobj.GetComponent<Nagoshi.EventStatus>().GetRate();
                int sum = money - rate;
                if (sum <= 0)
                {
                    return;
                }
                bool action = eventobj.GetComponent<Nagoshi.EventStatus>().GetIsAtion();
                if (action)
                {
                    switch (result)
                    {
                        case PlayerStatus.EventStatus.brige:
                        case PlayerStatus.EventStatus.gondola:
                        case PlayerStatus.EventStatus.scaffold:
                        case PlayerStatus.EventStatus.movebox:
                            isEffect = true;
                            playerStatusScript.SetMoney(sum);
                            playerStatusScript.SetIsAction(true);
                            playerAnimationScript.SetIsAction();
                            eventobj.GetComponent<Nagoshi.EventStatus>().Action();
                            seManager.GetComponent<SEManager>().PlaySe(1);
                            break;
                    }
                }
            }
        }
        void Jump()
        {
            if(playerStatusScript.GetIsJump())
            {
                playerStatusScript.SetIsJump(false);
                playerAnimationScript.SetIsJump(true);
                GetComponent<Rigidbody>().AddForce(Vector3.up * playerStatusScript.GetJumpForce());
               seManager.GetComponent<SEManager>().PlaySe(4);
            }
        }

        void ElevatorAction()
        {
            float value = Input.GetAxis("Vertical");
            if(!playerStatusScript.GetIsElevetorAction())
            {
                return;
            }
            Elevator elevator = playerStatusScript.GetElevator();
            elevator.Move(value);
        }

        //金消費エフェクト処理
        void EffectAction()
        {
            //各エフェクトを順に生成
            if (isEffect)
            {
                playerStatusScript.SetIsWalk(false);

                cnt += Time.deltaTime;
                if (cnt >= 0.0f && cnt < 1.0f)
                {
                    playerStatusScript.ActionEffect();
                    cnt = 3.5f;
                }
                else if (cnt >= 5.0f && cnt < 6.0f)
                {
                    playerStatusScript.ActionEffect();
                    cnt = 6.0f;
                }
                else if (cnt >= 6.0f && cnt < 7.0f)
                {
                    playerStatusScript.ActionEffect();
                    cnt = 7.0f;
                }
                else if (cnt >= 7.0f && cnt < 8.0f)
                {
                    playerStatusScript.ActionEffect();
                    cnt = 8.0f;
                }
                else if (cnt >= 9.0f)
                {
                    isEffect = false;
                    playerStatusScript.SetIsAction(false);
                    playerAnimationScript.SetIsAction();
                    cnt = 0.0f;
                }
            }
        }
    }
}