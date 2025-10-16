using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("¼³Á¤")]
    public Transform target;
    [SerializeField] private Vector2 offset = new Vector2(0.0f, 1.0f);
    [SerializeField] private float followSpeed = 5.0f;
    private bool followX = true;
    private bool isMaintrue = true;
    private bool istrue = false;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 targetPos = transform.position;

        if (followX)
        {
            targetPos.x = target.position.x + offset.x;
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed);
    }
}
