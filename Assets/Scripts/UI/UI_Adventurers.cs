using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UI_Adventurers : MonoBehaviour
{
    [SerializeField]
    private Set_Adventurer adventurerSet = default;

    [SerializeField]
    private VisualTreeAsset adventurerIcon = default;

    [SerializeField]
    private Sprite lostAdventurer = default;

    private List<VisualElement> adventurers = new List<VisualElement>();

    private void OnEnable()
    {
        adventurerSet.OnCountChanged += OnCountChanged;

        OnCountChanged();
    }

    private void OnDisable()
    {
        adventurerSet.OnCountChanged -= OnCountChanged;
    }

    private void OnCountChanged()
    {
        var doc = GetComponent<UIDocument>();
        var parent = doc.rootVisualElement.Q<VisualElement>("Adventurers");

        int childCount = parent.childCount;

        // remove surplus elements
        for (int i = adventurerSet.Count; i < childCount; ++i)
            parent.RemoveAt(i);

        for (int i = childCount; i < adventurerSet.Count; ++i)
            adventurerIcon.CloneTree(parent);

        adventurers.Clear();
        // do stuff to lives
        foreach (var child in parent.Children())
            adventurers.Add(child.Q<VisualElement>("Icon"));

        for(int i = 0; i < adventurerSet.Count; ++i)
		{
            adventurerSet[i].OnRescue -= OnAdventurerRescued;
            adventurerSet[i].OnRescue += OnAdventurerRescued;
		}

        OnAdventurerRescued();
    }

    private void OnAdventurerRescued()
	{
        for(int i = 0; i < adventurerSet.Count; ++i)
            adventurers[i].SetSprite((adventurerSet[i].IsRescued && adventurerSet[i].Icon) ? adventurerSet[i].Icon : lostAdventurer);
	}
}
