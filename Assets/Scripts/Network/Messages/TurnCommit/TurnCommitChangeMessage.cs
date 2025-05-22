using System;
using JetBrains.Annotations;
using Riptide;
using UnityEngine;

namespace Assets.Scripts.Network.Messages.TurnCommit
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
		/// Context of a related chair
		/// </summary>
		public int ChairContext { get; set; } = -1;

		/// <summary>
		/// Context of a related bar stool
		/// </summary>
		public int BarStoolContext { get; set; } = -1;
		
		/// <summary>
		/// Context of an affected card
		/// </summary>
		public int CardContext { get; set; } = -1;
		
		public void Serialize(Message message)
		{
			message.AddString(Action.ToString());
			message.AddInt(ChairContext);
			message.AddInt(BarStoolContext);
			message.AddInt(CardContext);
		}

		public void Deserialize(Message message)
		{
			Action = Enum.Parse<TurnCommitAction>(message.GetString());
			ChairContext = message.GetInt();
			BarStoolContext = message.GetInt();
			CardContext = message.GetInt();
		}
	}
}