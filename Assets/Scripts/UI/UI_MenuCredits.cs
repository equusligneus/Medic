using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UI_MenuCredits : MonoBehaviour, UI_IMenu
{
    [SerializeField]
    private VisualTreeAsset menu = default;

    [SerializeField]
    private TextAsset credits = default;

    public void Create()
	{
        var parent = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Panel");
        if (parent.childCount > 0)
            parent.RemoveAt(0);

        menu.CloneTree(parent);
		parent.Q<Button>("Btn_To_Main").clicked += OnBackToMain;
        parent.Q<Label>("Credits").text = credits.text;
	}

	private void OnBackToMain()
	{
        GetComponent<UI_MenuMain>().Create();
	}
}
