using System.Linq;
using Riptide;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Network.Messages.StartGame
{
	/// <summary>
	/// Handles game starting
	/// </summary>
	public static class StartGameHandler
	{
		/// <summary>
		/// Handles the StartGameMessage received from the network and initiates the game start process by invoking relevant functionalities.
		/// </summary>
		/// <param name="message">The message object containing serialized start game details.</param>
		[MessageHandler(1002)]
		private static void StartGameMessageHandler(Message message)
		{
			var obj = message.GetSerializable<StartGameMessage>();
			
			// Init Players
			LobbyStorage.Instance.ActivePlayers = obj.Players.ToList();
			Debug.Log($"Start Game with players: : {string.Join(',', obj.Players.Select(x => x.PlayerName))}");
			
			// Start Game
			Debug.Log($"Starting the Game. Lobby: {obj.LobbyName}");
			SceneManager.LoadScene("Game", LoadSceneMode.Additive);
			SceneManager.UnloadSceneAsync("MainMenu");
		}
	}
}