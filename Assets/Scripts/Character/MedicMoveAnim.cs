using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicMoveAnim : StateMachineBehaviour
{
	[SerializeField]
	private Ref<bool> isAlive = default;

	[SerializeField]
	private Ref<bool> isAwake = default;

	[SerializeField]
	private Ref<float> speed = default;

	[SerializeField]
	private Ref<bool> isPunching = default;
 
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool("IsAlive", isAlive.Get());
		animator.SetBool("IsAwake", isAwake.Get());
		animator.SetFloat("Speed", speed.Get());
		animator.SetBool("Attack", isPunching.Get());
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
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
