using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayEffect()
    {
        gameObject.SetActive(true);

        anim.Play("Explosion");

        StartCoroutine(DisableAnimationCo());
    }

    IEnumerator DisableAnimationCo()
    {
        // GetCurrentAnimatorStateInfo : 현재 애니메이션 상태 정보
        // 현재 애니메이션의 재생 길이만큼 기다려라
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // 풀에 다시 반환 추가하기
        Mangers.Pool.ReturnPool(this);
    }
}
