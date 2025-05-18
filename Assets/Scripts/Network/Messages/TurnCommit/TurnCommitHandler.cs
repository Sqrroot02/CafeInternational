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
		private static void TurnCommitMessageHandler(Message message)
		{
			var obj = message.GetSerializable<TurnCommitMessage>();
			Debug.Log("TurnCommitMessage has arrived. Game-Field will be updated with changed");

			foreach (var change in obj.Changes)
			{
				if (change.Action == TurnCommitAction.PlaceCardOnChair && change.ChairContext != null)
				{
					if (change.ChairContext.PlacedCard != null)
						Manager.PlayCard(change.ChairContext.PlacedCard.cardData.cardID, change.ChairContext.ChairID, -1);
				}

				if (change.Action == TurnCommitAction.PlaceCardOnBar && change.BarStoolContext != null)
				{
					if (change.BarStoolContext.PlacedCard != null)
						Manager.PlayCard(-1, -1, change.BarStoolContext.GetIndex());
				}
			}
		}
	}
}