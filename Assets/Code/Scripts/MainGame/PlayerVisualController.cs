using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private readonly int isMovingHash = Animator.StringToHash("IsWalking");

    public void RendererVisuals(Vector2 velocity)
    {
        var isMoving = velocity.x > 0.1f || velocity.x < -0.1f;

        animator.SetBool(isMovingHash, isMoving);
    }
}