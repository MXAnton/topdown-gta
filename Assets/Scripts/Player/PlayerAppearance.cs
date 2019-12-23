using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    private Animator animator;
    public string[] animStates;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationState(string currentAnimState)
    {
        animator.SetBool(currentAnimState, true);

        foreach (string state in animStates)
        {
            if (state != currentAnimState)
            {
                animator.SetBool(state, false);
            }
        }
    }
}
