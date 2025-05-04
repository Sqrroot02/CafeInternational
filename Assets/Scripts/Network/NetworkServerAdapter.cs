using Assets.Scripts.Models;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using Riptide;
using Riptide.Utils;
using UnityEngine;

/// <summary>
/// Singleton class that provides networking features to the Game as Server
/// </summary>
/// <remarks>
/// Only Host will use this adapter. Other participants, the Host included, must use the <see cref="NetworkClientAdapter"/> 
/// </remarks>
public class NetworkServerAdapter : MonoBehaviour
{
	private static NetworkServerAdapter _instance;

	/// <summary>
	/// Represents the current network session managed by the server.
	/// </summary>
	/// <remarks>
	/// Contains information about connected players and session state.
	/// </remarks>
	public Session Session { get; } = new();
	
	/// <summary>
	/// Returns the Instance of the Network Manager
	/// </summary>
	public static NetworkServerAdapter Instance
	{
		get => _instance;
		protected set
		{
			if (_instance == null)
				_instance = value;
			else if (_instance != value)
			{
				Debug.Log("Instance already set! Deconstructing object");
				Destroy(value);
			}
		}
	}
	
	/// <summary>
	/// Max. Client count
	/// </summary>
	[SerializeField]
	protected ushort MaxClient;
	
	/// <summary>
	/// Port 
	/// </summary>
	[SerializeField]
	protected ushort Port;
	
	/// <summary>
	/// Server, which will be used for operating in Multiplayer Mode
	/// </summary>
	public Server Server { get; private set; }

	private void Awake()
	{
		Instance = this;
	}
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
	{	
		// Enable Logging for Riptide 
		RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
		Debug.Log("NetworkServerAdapter 'Start()' has been called");
		
		Server = new Server();
		Server.ClientConnected += OnPlayerConnected;
		Server.MessageReceived += Distribute;
		Server.Start(Port, MaxClient);
	}

	/// <summary>
	/// Performs Broadcast to other Players if MSG-ID is over 1000 or Multicast on over 3000
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void Distribute(object sender, MessageReceivedEventArgs e)
	{
		switch (e.MessageId)
		{
			case > 3000:
				Server.SendToAll(e.Message, e.FromConnection.Id);
				break;
			case > 1000:
				Server.SendToAll(e.Message);
				break;
		}
	}

	private void OnPlayerConnected(object sender, ServerConnectedEventArgs e)
	{
		Debug.Log($"Player Connected: {e.Client.Id}!");
	}

	private void FixedUpdate()
	{
		if (Server == null)
		{
			Debug.LogError("No Server available!");
		}
		Server?.Update();
	}

	/// <summary>
	/// Stops Server before game will be quited
	/// </summary>
	private void OnApplicationQuit()
	{
		Server.Stop();
	}

	/// <summary>
	/// Handles player salutation
	/// </summary>
	/// <param name="fromClientId"></param>
	/// <param name="message"></param>
	[MessageHandler(1)]
	private static void HandlePlayerSalutationMessage(ushort fromClientId, Message message)
	{
		Debug.Log($"Received player salutation from {fromClientId}");
		var payload = message.GetSerializable<PlayerSalutationMessage>();
		var player = new Player(payload.PlayerName, 0);
		_instance.Session.Players.Add(player);
	}
	
}