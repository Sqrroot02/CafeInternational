using Assets.Scripts.Network.Adapter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	/// <summary>
	/// Behaviour for game menu 
	/// </summary>
	public class GameMenu : MonoBehaviour
	{
		public GameObject EscapeMenuUI;

		private Button _resumeButton;

		/// <summary>
		/// Hides the menu
		/// </summary>
		public void Resume()
		{
			EscapeMenuUI.SetActive(false);
		}

		/// <summary>
		/// Quits back to the Main Menu
		/// </summary>
		public void Quit()
		{
			Debug.Log("Disconnect Lobby");
			NetworkClientAdapter.Instance.Disconnect();
			if (NetworkServerAdapter.Instance.Server.IsRunning)
				NetworkServerAdapter.Instance.TearDown();
			SceneManager.LoadScene("MainMenu");
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				EscapeMenuUI.SetActive(!EscapeMenuUI.activeSelf);
			}
		}
	}
}