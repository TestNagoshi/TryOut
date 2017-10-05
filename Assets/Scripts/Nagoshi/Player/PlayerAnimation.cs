﻿//////////////////////
//製作者　名越大樹
//製作日　10月2日
//クラス名　プレイヤーのアニメーションに関するクラス
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nagoshi
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField]
        Nagoshi.PlayerStatus playerStatusScript;
        Animation anim;
        [SerializeField]
        Animator animControl;
        void Start()
        {
            anim = GetComponent<Animation>();
            animControl = GetComponent<Animator>();
        }

        /// <summary>
        /// 移動キーを押されたときに歩くアニメーションの処理
        /// </summary>
        public void SetIsWalk()
        {
            animControl.SetBool("isWalk", playerStatusScript.GetIsWalk());
        }

        public void SetIsJump()
        {
            bool isAction = !playerStatusScript.GetIsJump();
            animControl.SetBool("isJump", isAction);
        }

        public void SetIsAction()
        {
            Debug.Log("hoge");
            animControl.SetBool("isAction", playerStatusScript.GetIsAction());
        }
    }
}