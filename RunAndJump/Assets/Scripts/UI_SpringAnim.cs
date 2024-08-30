using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SpringAnim : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void StartAnim()
    {
        anim.SetBool("isPlay", true);
    }

    public void TimeStop()
    {
        Time.timeScale = 0.0f;
    }
}
