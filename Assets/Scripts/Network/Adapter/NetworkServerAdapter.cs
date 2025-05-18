using System;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Adapter;
using Assets.Scripts.Network.Messages;
using Assets.Scripts.Network.Messages.PlayerSalutation;
using Assets.Scripts.Network.Models;
using JetBrains.Annotations;
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
	[CanBeNull]
	public Session Session { get; private set; }
	
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
	public ushort MaxClient;
	
	/// <summary>
	/// Port 
	/// </summary>
	[SerializeField]
	public ushort Port;
	
	/// <summary>
	/// Server, which will be used for operating in Multiplayer Mode
	/// </summary>
	public Server Server { get; private set; }

	private void Awake()
	{
		DontDestroyOnLoad(this);
		Instance = this;
	}

	/// <summary>
	/// Run Game Server on the Host client
	/// </summary>
	public void RunServer(Session session)
	{
		Debug.Log("NetworkServerAdapter 'RunServer()' has been called");
		if (!Server.IsRunning)
		{
			Session = session;
			Server.Start(Port, MaxClient);	
			Debug.Log("Game Server has been started");
		}
		else
			Debug.Log("Game Server has been already started");
	}

	/// <summary>
	/// Initializes Server on startup. Server will stay idle until RunServer() has not been invoked 
	/// </summary>
	void Start()
	{
		Debug.Log("Initializes Server configuration");
		// Enable Logging for Riptide 
		RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
		
		Server = new Server();
		Server.ClientConnected += OnPlayerConnected;
		Server.MessageReceived += Distribute;
		Server.ClientDisconnected += ServerOnClientDisconnected;
	}

	/// <summary>
	/// Handles Player Disconnects. Removes the player from the session 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void ServerOnClientDisconnected(object sender, ServerDisconnectedEventArgs e)
	{
		_instance.Session?.RemovePlayer(e.Client.Id);
	}

	/// <summary>
	/// Tear down the game server on the host's client
	/// </summary>
	public void TearDown()
	{
		Debug.Log("Game Server will be terminated");
		Server.Stop();
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
				Debug.Log($"Distribute Data: [MessageID = {e.MessageId}]");
				Server.SendToAll(e.Message, e.FromConnection.Id);
				break;
			case > 1000:
				Debug.Log($"Distribute Data: [MessageID = {e.MessageId}]");
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
	private void OnApplicationQuit() => TearDown();

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
		var player = new Player
		{
			PlayerName = payload.PlayerName,
			ClientId = fromClientId,
			LobbyHost = false,
			IsBot = false,
			PlayerId = Guid.NewGuid().ToString()
		};
		
		Debug.Log($"Player {player.PlayerName} [{player.PlayerId}] will be added to session");
		_instance.Session?.AddPlayer(player);
	}
	
}