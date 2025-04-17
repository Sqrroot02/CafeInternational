using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	/// <summary>
	/// Behaviour for game menu 
	/// </summary>
	public class GameMenu : MonoBehaviour
	{
		private Button _resumeButton;

		/// <summary>
		/// Hides the menu
		/// </summary>
		public void HideMenu()
		{
			Debug.Log("GameMenu-Enabled: New State -> HIDDEN");
			enabled = false;
			gameObject.SetActive(false);
		}
		
		/// <summary>
		/// Init components
		/// </summary>
		void Start()
		{
			Debug.Log("Init Game Menu");
			InitResumeButton();
		}

		/// <summary>
		/// Init resume button
		/// </summary>
		void InitResumeButton()
		{
			Debug.Log("Init Game Menu -> Resume Button");
			_resumeButton = transform.Find("GameMenuBackground").Find("ResumeButton").GetComponent<Button>();
			_resumeButton.onClick.AddListener(HideMenu);
		}
	}
}