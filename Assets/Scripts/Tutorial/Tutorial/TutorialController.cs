using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
	[SerializeField]
	private	List<TutorialBase>	tutorials;
	[SerializeField]
	private	string				nextSceneName = "";

	private TutorialBase		currentTutorial = null;
	private	int					currentIndex = -1;

	private void Start()
	{
		SetNextTutorial();
	}

	private void Update()
	{
		if ( currentTutorial != null )
		{
			currentTutorial.Execute(this);
		}
	}

	public void SetNextTutorial()
	{
		if ( currentTutorial != null )
		{
			currentTutorial.Exit();
		}

		if ( currentIndex >= tutorials.Count-1 )
		{
			CompletedAllTutorials();
			return;
		}

		currentIndex ++;
		currentTutorial = tutorials[currentIndex];

		currentTutorial.Enter();
	}

	public void CompletedAllTutorials()
	{
		currentTutorial = null;

		Debug.Log("Complete All");

		if ( !nextSceneName.Equals("") )
		{
			SceneManager.LoadScene(nextSceneName);
		}
	}
}

