using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Network.Messages.TurnCommit;
using JetBrains.Annotations;
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
			if (ValidateChairChange(chair))
			{
				Debug.Log("Add Chair Change");		
				UpdateChair.Add(chair);
			}
			
		}

		private bool ValidateChairChange([CanBeNull] Chair chair) =>
			chair != null && 
			chair.ChairID >= 0 && 
			chair.PlacedCard.cardData.cardID >= 0 && 
			UpdateChair.All(x => x.ChairID != chair.ChairID);
		
		private bool ValidateBarStoolChange([CanBeNull] BarStool stool) =>
			stool != null && 
			stool.GetIndex() >= 0 && 
			stool.PlacedCard.cardData.cardID >= 0 && 
			UpdateChair.All(x => x.ChairID != stool.GetIndex());
		
		/// <summary>
		/// Adds refreshed data of a bar stool
		/// </summary>
		/// <param name="barStool"></param>
		public void AddBarStool(BarStool barStool)
		{
			if (ValidateBarStoolChange(barStool))
			{
				Debug.Log("Add Bar-Stool Change");	
				UpdateBarStool.Add(barStool);	
			}
		}

		public List<TurnCommitChangeMessage> MessagesOfChair =>
			UpdateChair.Select(x => new TurnCommitChangeMessage()
			{
				Action = TurnCommitAction.PlaceCardOnChair,
				ChairContext = x.ChairID,
				CardContext = x.PlacedCard.cardData.cardID
			}).ToList();

		public List<TurnCommitChangeMessage> MessagesOfBarStool =>
			UpdateBarStool.Select(x => new TurnCommitChangeMessage()
			{
				Action = TurnCommitAction.PlaceCardOnBar,
				BarStoolContext = x.GetIndex(),
				CardContext = x.PlacedCard.cardData.cardID
			}).ToList();

		/// <summary>
		/// Pulls the final message. Use only on End-Turn. The running History will be cleared after invoking this methode
		/// </summary>
		/// <param name="currentPlayer"></param>
		/// <param name="nextPlayer"></param>
		/// <returns></returns>
		public static TurnCommitMessage PullMessage(Player currentPlayer)
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
				Player = currentPlayer,
			};
			
			return message;
		}
	}
}