using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Network.Messages.TurnCommit;
using UnityEngine;

namespace Assets.Scripts.Game
{
	/// <summary>
	/// History that contains all changes that has been made during a single turn 
	/// </summary>
	public class TurnHistory
	{
		/// <summary>
		/// Contains the current History
		/// </summary>
		public static TurnHistory CurrentTurnHistory { get; set; } = new();

		/// <summary>
		/// Contains all placed cards on a chair
		/// </summary>
		public List<Chair> UpdateChair { get; } = new();

		/// <summary>
		/// Contains all placed cards on bar stools
		/// </summary>
		public List<BarStool> UpdateBarStool { get; } = new();

		/// <summary>
		/// Add refreshed data of a single chair
		/// </summary>
		/// <param name="chair"></param>
		public void AddChair(Chair chair)
		{
			if (!UpdateChair.Contains(chair))
				UpdateChair.Add(chair);
		}

		/// <summary>
		/// Adds refreshed data of a bar stool
		/// </summary>
		/// <param name="barStool"></param>
		public void AddBarStool(BarStool barStool)
		{
			if (!UpdateBarStool.Contains(barStool))
				UpdateBarStool.Add(barStool);
		}

		public List<TurnCommitChangeMessage> MessagesOfChair =>
			UpdateChair.Select(x => new TurnCommitChangeMessage()
			{
				Action = TurnCommitAction.PlaceCardOnChair,
				ChairContext = x.ChairID,
				CardContext = x.PlacedCard?.cardData?.cardID ?? -1
			}).ToList();

		public List<TurnCommitChangeMessage> MessagesOfBarStool =>
			UpdateBarStool.Select(x => new TurnCommitChangeMessage()
			{
				Action = TurnCommitAction.PlaceCardOnBar,
				BarStoolContext = x.GetIndex(),
				CardContext = x.PlacedCard?.cardData?.cardID ?? -1
			}).ToList();

		/// <summary>
		/// Pulls the final message. Use only on End-Turn. The running History will be cleared after invoking this methode
		/// </summary>
		/// <param name="currentPlayer"></param>
		/// <param name="nextPlayer"></param>
		/// <returns></returns>
		public static TurnCommitMessage PullMessage(Player currentPlayer, Player nextPlayer)
		{
			var changes = CurrentTurnHistory.MessagesOfBarStool;
			changes.AddRange(CurrentTurnHistory.MessagesOfChair);
			
			// Log changes
			Debug.Log($"Number of Changes: {changes.Count}");
			var changesString = string.Join('\n',
				changes.Select(x =>
					$"{x.Action} -> [BarStool: {x.BarStoolContext}] [Chair: {x.ChairContext}] [Card: {x.CardContext}]"));
			Debug.Log($"Changes: {changesString}");
			
			// Clear History
			CurrentTurnHistory = new TurnHistory();
			
			// Build Message
			var message = new TurnCommitMessage
			{
				Changes = changes.ToArray(),
				NextPlayer = nextPlayer,
				Player = currentPlayer,
			};
			
			return message;
		}
	}
}