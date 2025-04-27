using Assets.Scripts.Network;
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
		
		Server = new Server();
		Server.ClientConnected += OnPlayerConnected;
		Server.Start(Port, MaxClient);
	}

	private void OnPlayerConnected(object sender, ServerConnectedEventArgs e)
	{
		Session.Players.Add(new PlayerConnection()
		{
			ClientConnection = e.Client
		});
	}

	/// <summary>
	/// Stops Server before game will be quited
	/// </summary>
	private void OnApplicationQuit()
	{
		Server.Stop();
	}
	
}