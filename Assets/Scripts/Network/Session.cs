using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Network
{
	/// <summary>
	/// Represents a network session containing a list of connected players.
	/// </summary>
	/// <remarks>
	/// This class manages the collection of players participating in the session.
	/// It can be used to reference or manipulate player-specific data collectively.
	/// </remarks>
	public class Session
	{
		/// <summary>
		/// The name of the opened Session
		/// </summary>
		public string Name { get; set; }
		
		public Session()
		{
			Debug.Log("Session has been created");
			Players.CollectionChanged += OnPlayersChanged;
		}

		private void OnPlayersChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Debug.Log("Players Collection has been changed");	
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				Debug.Log($"Player {e.NewItems[0]} has been added to the session");	
			}
		}

		/// <summary>
		/// Gets or sets the collection of players in the current session.
		/// </summary>
		/// <remarks>
		/// Represents a list of <see cref="PlayerConnection"/> objects associated with the session.
		/// Each player contains information such as name, IP address, and port.
		/// </remarks>
		public ObservableCollection<PlayerConnection> Players { get; set; } = new();
	}
}