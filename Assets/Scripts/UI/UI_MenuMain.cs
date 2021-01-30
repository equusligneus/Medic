using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UI_MenuMain : MonoBehaviour, UI_IMenu
{
	[SerializeField]
	private VisualTreeAsset menu = default;

	public void Create()
	{
		var parent = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Panel");
		if(parent.childCount > 0)
			parent.RemoveAt(0);

		menu.CloneTree(parent);
		parent.Q<Button>("Btn_Start").clicked += OnStartGame;
		parent.Q<Button>("Btn_Credits").clicked += OnCredits;
		parent.Q<Button>("Btn_Exit").clicked += OnExit;
	}

	private void OnEnable()
	{
		Create();	
	}

	private void OnStartGame()
		=> SceneManager.LoadScene("Level_01");

	private void OnCredits()
	{
		GetComponent<UI_MenuCredits>().Create();
	}

	private void OnExit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif
	}
}
