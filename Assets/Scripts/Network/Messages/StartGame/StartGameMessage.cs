using System.Linq;
using Assets.Scripts.Models;
using Riptide;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Network.Messages.StartGame
{
	/// <summary>
	/// Represents a network message to start a game, containing relevant game state information.
	/// </summary>
	public class StartGameMessage : IMessageSerializable
	{
		public Player[] Players { get; set; }
		public string LobbyName { get; set; }
		public Player Starter { get; set; }
		
		public void Serialize(Message message)
		{
			message.AddSerializables(Players);
			message.AddString(LobbyName);
			message.AddSerializable(Starter);
		}

		public void Deserialize(Message message)
		{
			Players = message.GetSerializables<Player>();
			LobbyName = message.GetString();
			Starter = message.GetSerializable<Player>();
		}

		/// <summary>
		/// Handles the StartGameMessage received from the network and initiates the game start process by invoking relevant functionalities.
		/// </summary>
		/// <param name="message">The message object containing serialized start game details.</param>
		[MessageHandler(1002)]
		private static void StartGameMessageHandler(Message message)
		{
			var obj = message.GetSerializable<StartGameMessage>();
			Debug.Log($"Starting the Game. Lobby: {obj.LobbyName}");
			
			SceneManager.LoadScene("Game", LoadSceneMode.Additive);
			SceneManager.UnloadSceneAsync("MainMenu");
			
			Models.PlayersGameUtil.ActivePlayers = obj.Players.ToList();
		}
	}
}