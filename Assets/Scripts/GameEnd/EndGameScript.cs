using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
	const string END_SCREEN = "GameEndScreen";

    [SerializeField]
    private Condition winCondition = default;

    [SerializeField]
    private Condition loseCondition = default;

	[SerializeField]
	private Ref_Int gameResult = default;

	[SerializeField]
	private float sceneSwitchDelay = 2f;

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

		StartCoroutine(LoadEndScene());

	}

	private void WinCondition_OnConditionRaised()
	{
		gameResult.Set(1);
		StartCoroutine(LoadEndScene());
	}


	private IEnumerator LoadEndScene()
	{
		yield return new WaitForSeconds(sceneSwitchDelay);
		SceneManager.LoadScene(END_SCREEN);
	}
}
