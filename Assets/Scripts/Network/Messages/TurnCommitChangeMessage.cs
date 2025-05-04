using System;
using JetBrains.Annotations;
using Riptide;
using UnityEngine;

namespace Assets.Scripts.Network.Messages
{
	/// <summary>
	/// Message that notifies about a single change during a player's turn
	/// </summary>
	public class TurnCommitChangeMessage : IMessageSerializable
	{
		/// <summary>
		/// The performed action 
		/// </summary>
		public TurnCommitAction Action { get; set; }
		
		/// <summary>
		/// Context of a related card
		/// </summary>
		[CanBeNull]
		public Card CardContext { get; set; }
		
		/// <summary>
		/// Context of a related chair
		/// </summary>
		[CanBeNull]
		public Chair ChairContext { get; set; }
		
		/// <summary>
		/// Context of a related bar stool
		/// </summary>
		[CanBeNull]
		public BarStool BarStoolContext { get; set; }
		
		/// <summary>
		/// Context of a related table 
		/// </summary>
		[CanBeNull]
		public Table TableContext { get; set; }
		
		public void Serialize(Message message)
		{
			message.AddString(Action.ToString());
			switch (Action)
			{
				case TurnCommitAction.PlaceCardOnBar:
					SerializeActionCardOnBar(message);
					break;
				case TurnCommitAction.PlaceCardOnChair:
					SerializeActionCardOnChair(message);
					break;
			}
		}

		public void Deserialize(Message message)
		{
			Action = Enum.Parse<TurnCommitAction>(message.GetString());
			switch (Action)
			{
				case TurnCommitAction.PlaceCardOnBar:
					DeserializeActionCardOnBar(message);
					break;
				case TurnCommitAction.PlaceCardOnChair:
					DeserializeActionCardOnChair(message);
					break;
			}
		}

		#region Deserialization

		/// <summary>
		/// Handles deserialization for Action <see cref="TurnCommitAction"/>.PlaceCardOnBar.
		/// </summary>
		/// <param name="message">The message containing serialized data for the PlaceCardOnBar action.</param>
		private void DeserializeActionCardOnBar(Message message)
		{
			CardContext = message.GetSerializable<Card>();
			BarStoolContext = message.GetSerializable<BarStool>();
		}

		/// <summary>
		/// Handles deserialization for Action <see cref="TurnCommitAction"/>.PlaceCardOnChair.
		/// </summary>
		/// <param name="message">The message containing serialized data for the PlaceCardOnChair action.</param>
		private void DeserializeActionCardOnChair(Message message)
		{
			CardContext = message.GetSerializable<Card>();
			TableContext = message.GetSerializable<Table>();
			ChairContext = message.GetSerializable<Chair>();
		}

		#endregion
		

		#region Serialization

		/// <summary>
		/// Handles serialization for Action <see cref="TurnCommitAction"/>.PlaceCardOnBar
		/// </summary>
		private void SerializeActionCardOnBar(Message message)
		{
			if (CardContext != null && BarStoolContext != null)
			{
				message.AddSerializable(CardContext);
				message.AddSerializable(BarStoolContext);
			}
			else
			{
				const string msg = "TurnCommitAction.PlaceCardOnBar required CardContext and BarStoolContext";
				Debug.LogError(msg);
				throw new ArgumentNullException($"{nameof(CardContext)}, {nameof(BarStoolContext)}", msg);
			}
		}
		
		/// <summary>
		/// Handles serialization for Action <see cref="TurnCommitAction"/>.PlaceCardOnChair
		/// </summary>
		private void SerializeActionCardOnChair(Message message)
		{
			if (CardContext != null && ChairContext != null && TableContext != null)
			{
				message.AddSerializable(CardContext);
				message.AddSerializable(TableContext);
				message.AddSerializable(ChairContext);
			}
			else
			{
				const string msg = "TurnCommitAction.PlaceCardOnChair required CardContext and TableContext";
				Debug.LogError(msg);
				throw new ArgumentNullException($"{nameof(CardContext)} and {nameof(TableContext)}", msg);
			}
		}

		#endregion
	}
}