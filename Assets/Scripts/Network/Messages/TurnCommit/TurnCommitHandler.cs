using System;
using System.Linq;
using Assets.Scripts.Game;
using Assets.Scripts.Models;
using Assets.Scripts.Network.Messages.StartGame;
using Riptide;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Network.Messages.TurnCommit
{
	public static class TurnCommitHandler
	{
		public static PlayerManager Manager;
		
		/// <summary>
		/// Handler for turn commit messages 
		/// </summary>
		/// <param name="message"></param>
		[MessageHandler(1003)]
		public static void TurnCommitMessageHandler(Message message)
		{
			var obj = message.GetSerializable<TurnCommitMessage>();
			
			// Ignore own message
			if (obj.Player.PlayerId == LobbyStorage.Instance.ClientPlayerId)
				return;
			
			Debug.Log($"TurnCommitMessage has arrived. Game-Field will be updated with {obj.Changes.Length} changes.");
		
			foreach (var change in obj.Changes)
			{
				if (change.Action == TurnCommitAction.PlaceCardOnChair)
				{
					if (change.ChairContext > -1 && change.CardContext > -1)
					{
						if (change.JokerIdentity != null)
						{
							var identity = Enum.Parse<Nationality>(change.JokerIdentity);
							Manager.PlayCard(change.CardContext, change.ChairContext, -1, obj.Player.PlayerId, identity);	
						}
						else
						{
							Manager.PlayCard(change.CardContext, change.ChairContext, -1, obj.Player.PlayerId);	
						}
					}
				}

				if (change.Action == TurnCommitAction.PlaceCardOnBar)
				{
					if (change.BarStoolContext > -1 && change.CardContext > -1)
						Manager.PlayCard(change.CardContext, -1, change.BarStoolContext, obj.Player.PlayerId);
				}
			}
			PlayerManager.Instance.UpdatePlayer();
		}
	}
}