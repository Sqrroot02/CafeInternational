using System;
using System.Net;
using System.Net.Sockets;
using Riptide;
using Riptide.Utils;
using UnityEngine;

namespace Assets.Scripts.Network.Adapter
{
	/// <summary>
	/// Singleton class that provides networking features to the Game as single client
	/// </summary>
	public class NetworkClientAdapter : MonoBehaviour
	{
		private static NetworkClientAdapter _instance;

		/// <summary>
		/// Event triggered when the client successfully connects to the server.
		/// </summary>
		public event EventHandler Connected = delegate { };
	
		public event EventHandler Disconnected = delegate { };
	
		/// <summary>
		/// Retuns the Instance of the Network Manager
		/// </summary>
		public static NetworkClientAdapter Instance
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
		/// Port 
		/// </summary>
		[SerializeField]
		public string IpAddress;
    
		/// <summary>
		/// Port 
		/// </summary>
		[SerializeField]
		public ushort Port;
    
		/// <summary>
		/// Returns the Address of the socket as a connection string
		/// </summary>
		protected string SocketAddress ()
		{
			if (IPAddress.TryParse(IpAddress, out IPAddress address))
				if (address.AddressFamily == AddressFamily.InterNetworkV6)
					return $"[{IpAddress}]:{Port}";
			return $"{IpAddress}:{Port}";
		}
    
		/// <summary>
		/// Client Socket of the game participant
		/// </summary>
		public Client Client { get; private set; }
    
		// Start is called once before the first execution of Update after the MonoBehaviour is created
		private void Start()
		{	
			Debug.Log("Initialize NetworkClientAdapter");
		
			// Enable Logging for Riptide 
			RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
			Client = new Client();
			Client.Connected += ClientOnConnected;
			Client.ConnectionFailed += ClientOnConnectionFailed;
			Client.Disconnected += ClientOnDisconnected;
		}

		private void ClientOnDisconnected(object sender, DisconnectedEventArgs e)
		{
			Disconnected?.Invoke(sender, e);
			Debug.Log($"Client connection to server: {SocketAddress()} has been disconnected");
		}

		private void ClientOnConnectionFailed(object sender, ConnectionFailedEventArgs e)
		{
			Debug.LogError($"Client failed connection to server: {SocketAddress()}");
		}
    
		private void ClientOnConnected(object sender, EventArgs e)
		{
			Connected?.Invoke(this, EventArgs.Empty);
			Debug.Log($"Client connected to server: {SocketAddress()}");
		
		}

		/// <summary>
		/// Connects to the Server and sends a salutation with the name of the new player
		/// </summary>
		public void Connect()
		{
			Debug.Log($"Connect NetworkClientAdapter to {SocketAddress()}");
			Client.Connect(SocketAddress());
		}

		/// <summary>
		/// Destroy connection the current game server 
		/// </summary>
		public void Disconnect()
		{
			Client.Disconnect();
		}

		/// <summary>
		/// Sends a Message to the connected Server
		/// </summary>
		/// <param name="message"></param>
		public void SendMessage(Message message)
		{
			if (Client.IsConnected)
			{
				Client.Send(message);
				Debug.Log($"Sent message to server: {SocketAddress()}");
			}
			else
			{
				Debug.LogError($"No Connection to server: {SocketAddress()} has been established. Message has been refused");
			}
		}

		private void FixedUpdate()
		{
			Client?.Update();
		}

		private void Awake()
		{
			DontDestroyOnLoad(this);
			Instance = this;
		}

		private void OnApplicationQuit()
		{
			Client.Disconnect();
		}
	}
}