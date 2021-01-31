using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    [SerializeField]
    private Condition winCondition = default;

    [SerializeField]
    private Condition loseCondition = default;

	[SerializeField]
	private Ref_Int gameResult = default;

	private void OnEnable()
	{
		gameResult.Set(0);
		winCondition.OnConditionRaised += WinCondition_OnConditionRaised;
		loseCondition.OnConditionRaised += LoseCondition_OnConditionRaised;
	}

	private void OnDisable()
	{
		winCondition.OnConditionRaised -= WinCondition_OnConditionRaised;
		loseCondition.OnConditionRaised -= LoseCondition_OnConditionRaised;
	}

	private void LoseCondition_OnConditionRaised()
	{
		gameResult.Set(-1);
		SceneManager.LoadScene("GameOverScene");
	}

	private void WinCondition_OnConditionRaised()
	{
		gameResult.Set(1);
		SceneManager.LoadScene("GameOverScene");
	}

}
