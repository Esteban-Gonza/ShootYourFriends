using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform gunPivotTr;
    [SerializeField] private Transform canvasTr;

    private readonly int isMovingHash = Animator.StringToHash("IsWalking");
    private bool isFacingRight = true;
    private bool init;
    private Vector3 originalPlayerScale;
    private Vector3 originalGunPivotScale;
    private Vector3 originalCanvasScale;

    private void Start()
    {
        originalPlayerScale = this.transform.localScale;
        originalGunPivotScale = gunPivotTr.transform.localScale;
        originalCanvasScale = canvasTr.transform.localScale;

        init = true;
    }

    public void RendererVisuals(Vector2 velocity)
    {
        if (!init) return;

        var isMoving = velocity.x > 0.1f || velocity.x < -0.1f;

        animator.SetBool(isMovingHash, isMoving);
    }

    public void UpdateScaleTransforms(Vector2 velocity)
    {
        if (!init) return;

        if (velocity.x > 0.1f)
        {
            isFacingRight = true;
        }
        else if(velocity.x < -0.1f)
        {
            isFacingRight = false;
        }

        SetObjectLocalScaleBasedOnDirection(gameObject, originalPlayerScale);
        SetObjectLocalScaleBasedOnDirection(canvasTr.gameObject, originalCanvasScale);
        SetObjectLocalScaleBasedOnDirection(gunPivotTr.gameObject, originalGunPivotScale);
    }

    private void SetObjectLocalScaleBasedOnDirection(GameObject obj, Vector3 originalScale)
    {
        var yValue = originalScale.y;
        var zValue = originalScale.z;
        var xValue = isFacingRight ? originalScale.x : -originalScale.x;
        obj.transform.localScale = new Vector3(xValue, yValue, zValue);
    }
}