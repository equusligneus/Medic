﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreakTime : StateMachineBehaviour
{
    private KIController contr;
    private float internalBreakTime = 0.0f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        contr = animator.GetComponent<KIController>();
        contr.NextWaypoint();
        internalBreakTime = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(contr != null)
        {
            if (contr.PlayerInViewSpace())
            {
                animator.SetBool("BreakTime", false);
                animator.SetBool("PlayerInView", true);
            }
            else
            {
                if (internalBreakTime < contr.BreakTime)
                {
                    internalBreakTime += Time.deltaTime;
                }
                else
                {
                    animator.SetBool("BreakTime", false);
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
