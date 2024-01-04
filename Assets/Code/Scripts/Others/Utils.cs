using System.Collections;
using UnityEngine;

public static class Utils
{
    public static IEnumerator PlayAnimSetStateWhenFinished(GameObject parent, Animator animator, string clipName, bool activeState = true)
    {
        animator.Play(clipName);
        var animationLenght = animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSecondsRealtime(animationLenght);
        parent.SetActive(activeState);
    }
}
