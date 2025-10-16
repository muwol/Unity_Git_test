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
        // GetCurrentAnimatorStateInfo : ���� �ִϸ��̼� ���� ����
        // ���� �ִϸ��̼��� ��� ���̸�ŭ ��ٷ���
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Ǯ�� �ٽ� ��ȯ �߰��ϱ�
        Mangers.Pool.ReturnPool(this);
    }
}
