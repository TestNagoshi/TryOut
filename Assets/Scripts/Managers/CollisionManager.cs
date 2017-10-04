﻿//////////////////////
//製作者　名越大樹
//製作日　10月2日
//クラス名　あたり判定の処理をするクラス　
//////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    [SerializeField]
    UIManager UIScript;
    [SerializeField]
    Nagoshi.InstanceManager InstanceScript;
    [SerializeField]
    Yamaji.UI UI;
    float cnt = 0.0f;
    /// <summary>
    /// プレイやーがほかのオブジェクト衝突した時の処理
    /// </summary>
    public void HitPlayer(GameObject playerobj,GameObject hitobj)
    {
        //イベントのオブジェクトに衝突した時
        if (hitobj.tag == "Event")
        {
            playerobj.GetComponent<Nagoshi.PlayerStatus>().SetEventObj(hitobj);
            GameObject obj = InstanceScript.InstanceObjects(0, hitobj.transform.position + Vector3.up * 2);
            Destroy(obj, 3.0f);
        }
        //ゴールオブジェクトに衝突した時
        else if (hitobj.tag == "Goal")
        {
            SceneManager.LoadScene("ResultScene");
            //Debug.Log("ゴール！");
        }
        //宝に衝突した時
        else if (hitobj.tag == "Tresure")
        {
            int money = playerobj.GetComponent<Nagoshi.PlayerStatus>().GetMoney();
            money += 500;
            playerobj.GetComponent<Nagoshi.PlayerStatus>().SetMoney(money);
            UIScript.SetMoneyValue(money);
            Debug.Log(money);
            Destroy(hitobj);
        }
        //ギミックに衝突した時
        else if (hitobj.tag == "Gimmick")
        {
            int hp = playerobj.GetComponent<Nagoshi.PlayerStatus>().GetHp();
            hp -= 20;
            playerobj.GetComponent<Nagoshi.PlayerStatus>().SetHp(hp);
            UI.HpGauge(hp);
            Debug.Log("Damage");
        }
        //敵と衝突した時
        else if (hitobj.tag == "Enemy")
        {
            int hp = playerobj.GetComponent<Nagoshi.PlayerStatus>().GetHp();
            hp -= 30;
            playerobj.GetComponent<Nagoshi.PlayerStatus>().SetHp(hp);
            UI.HpGauge(hp);
            Destroy(hitobj);
        }
        

        //地面と接した時
        else if(hitobj.tag == "Ground")
        {
            playerobj.GetComponent<Nagoshi.PlayerStatus>().SetIsJump(true);
        }

        //ゴンドラと接した時
        else if (hitobj.tag == "Gondola")
        {
           Vector3 copyscale = playerobj.transform.localScale;
           playerobj.transform.parent = hitobj.transform;
            playerobj.transform.localScale = copyscale;
        }
    }

    public void HitCollision(GameObject playerobj,GameObject hitobj)
    {
        if (hitobj.gameObject.tag == "Ground")
        {
            playerobj.GetComponent<Nagoshi.PlayerStatus>().SetIsJump(true);
        }

        else if (hitobj.gameObject.tag == "Fook")
        {
            playerobj.transform.parent = hitobj.transform;
        }
    }

    public void ExitCollsion(GameObject playerobj, GameObject exitobj)
    {
        if (exitobj.tag == "Gondola")
        {
            playerobj.transform.parent = null;
        }
    }
    public void StayCollision(GameObject playerobj, GameObject stayObj)
    {
        //毒エリアに衝突した時
        if (stayObj.tag == "Smog")
        {
            cnt += Time.deltaTime;
            Debug.Log(cnt);
            int hp = playerobj.GetComponent<Nagoshi.PlayerStatus>().GetHp();
            if (cnt > 1.0f)
            {
                hp -= 5;
                cnt = 0.0f;
            }
            playerobj.GetComponent<Nagoshi.PlayerStatus>().SetHp(hp);
            UI.HpGauge(hp);
        }
    }
}
