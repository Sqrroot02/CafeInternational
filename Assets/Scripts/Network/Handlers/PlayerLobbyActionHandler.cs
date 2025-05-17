using Assets.Scripts.Network.Messages.PlayerLobbyAction;
using JetBrains.Annotations;
using Riptide;
using UnityEngine;

namespace Assets.Scripts.Network.Handlers
{
	/// <summary>
	/// Handler for player lobby actions
	/// </summary>
	public static class PlayerLobbyActionHandler
	{
		[CanBeNull] 
		public static LobbyPanelManager Manager;
		
		/// <summary>
		/// Handles player actions
		/// </summary>
		/// <param name="message"></param>
		[MessageHandler(1001)]
		private static void Handle(Message message)
		{
			if (Manager == null)
			{
				Debug.Log("Cannot find LobbyPanelManager. Message will be ignored.");
				return;
			}
			
			var obj = message.GetSerializable<PlayerLobbyActionMessage>();
			Manager.LobbyUpdate(obj);
		}
	}
}