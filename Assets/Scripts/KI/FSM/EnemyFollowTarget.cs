using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowTarget : StateMachineBehaviour
{
    KIController contr;
    bool animationBreak = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        contr = animator.GetComponent<KIController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(contr != null && !animationBreak)
        {
            //contr.Move();
            if (contr.Agent.MoveTo(contr.currentTargetPosition))
            {
                if (contr.PlayerInViewSpace())
                {
                    animator.SetBool("PlayerInView", true);
                    if (contr.AttackAbility.IsPlayerInRange(contr.Player.Get()) && contr.PlayerAwake.Get())
                    {
                        animator.SetBool("Attack", true);
                        //animationBreak = true;
                        //contr.AttackAbility.AttackPlayer(contr.Player);
                    }
                }
                else
                {
                    animator.SetBool("PlayerInView", false);
                    animator.SetBool("NoValidPath", false);
                }
            }
            else
            {
                contr.CantReach = true;
                animator.SetBool("PlayerInView", false);
                animator.SetBool("NoValidPath", true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationBreak = true;
    }

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
