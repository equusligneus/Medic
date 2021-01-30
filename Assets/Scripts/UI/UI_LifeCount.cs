using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UI_LifeCount : MonoBehaviour
{
	[SerializeField]
	private Ref<int> currentLives = default;

	[SerializeField]
	private Ref<int> maxLives = default;

	[SerializeField]
	private VisualTreeAsset lifeIcon = default;

	[SerializeField]
	private Sprite lifeEmpty = default;

	[SerializeField]
	private Sprite lifeFull = default;

	private List<VisualElement> lives = new List<VisualElement>();

	// Start is called before the first frame update
	void Start()
	{

	}

	private void OnEnable()
	{
		currentLives.OnChanged += OnLivesChanged;
		maxLives.OnChanged += OnMaxLivesChanged;

		OnMaxLivesChanged();
	}

	private void OnDisable()
	{
		currentLives.OnChanged -= OnLivesChanged;
		maxLives.OnChanged -= OnMaxLivesChanged;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnLivesChanged()
	{
		for (int i = 0; i < maxLives.Get(); ++i)
			lives[i].SetSprite(i < currentLives.Get() ? lifeFull : lifeEmpty);
	}

	private void OnMaxLivesChanged()
	{
		var doc = GetComponent<UIDocument>();
		var parent = doc.rootVisualElement.Q<VisualElement>("Lives");

		int childCount = parent.childCount;

		// remove surplus elements
		for (int i = maxLives.Get(); i < childCount; ++i)
			parent.RemoveAt(i);

		for (int i = childCount; i < maxLives.Get(); ++i)
			lifeIcon.CloneTree(parent);

		lives.Clear();
		// do stuff to lives
		foreach (var child in parent.Children())
			lives.Add(child.Q<VisualElement>("Icon"));

		OnLivesChanged();
	}
}
