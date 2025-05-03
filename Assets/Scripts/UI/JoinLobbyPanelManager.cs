using System;
using Assets.Scripts.Network.Messages;
using Riptide;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the UI panel and interactions for joining a game lobby, including input handling for
/// lobby IP and port, connecting to the network server, and transitioning the UI to the lobby view.
/// </summary>
/// <remarks>
/// This class contains functionality to collect user inputs for the lobby's IP address and port,
/// validate and use those inputs to establish a connection with the server, and handle UI resets.
/// </remarks>
public class JoinLobbyPanelManager : MonoBehaviour
{
	public TMP_InputField lobbyIPTMP;

	public TMP_InputField lobbyPortTMP;

	public Button joinLobbyButton;

	private const string LobbyIpTmpStandardValue = "127.0.0.1";
	private const string LobbyPortTmpStandardValue = "57967";

	void Start()
	{
		lobbyIPTMP.onValueChanged.AddListener(LobbyNameInput_TMP_ValueChanged);
		DisableCreateLobbyButton();
	}

	void LobbyNameInput_TMP_ValueChanged(string newValue)
	{
		if (newValue != LobbyIpTmpStandardValue && !string.IsNullOrEmpty(newValue))
		{
			joinLobbyButton.interactable = true;
		}
	}

	/// <summary>
	/// Attempts to join a lobby by connecting to a server using the IP address and port entered in the UI.
	/// If a valid connection is established, the method transitions the UI to the lobby menu and updates the
	/// lobby connection details within the `MainMenuManager`.
	/// </summary>
	/// <remarks>
	/// The method retrieves the IP address and port values from the corresponding UI input fields.
	/// It validates the entered port against a standard value and establishes a connection using the
	/// `NetworkClientAdapter` singleton. If the connection succeeds, the lobby-specific information
	/// such as lobby IP, port, and a placeholder lobby name are updated and the lobby view is displayed.
	/// </remarks>
	public void JoinLobby()
	{
		var enteredIp = lobbyIPTMP.text;
		var enteredPort = lobbyPortTMP.text;

		Debug.Log("Entered ip: " + enteredIp);
		Debug.Log("Entered port: " + enteredPort);
        
		// Establish connection
		NetworkClientAdapter.Instance.IpAddress = enteredIp;
		NetworkClientAdapter.Instance.Port = Convert.ToUInt16(enteredPort);
		
		NetworkClientAdapter.Instance.Connected += OnConnected;    
		NetworkClientAdapter.Instance.Connect();
		
	}

	/// <summary>
	/// Handles the event triggered when the client successfully connects to the server.
	/// Updates the lobby connection details within the `MainMenuManager`, logs the connection
	/// details, and displays the lobby menu UI.
	/// </summary>
	/// <param name="sender">The source of the event. Typically, this is the instance of the `NetworkClientAdapter` that triggered the event.</param>
	/// <param name="e">The event arguments containing details about the connection event.</param>
	private void OnConnected(object sender, EventArgs e)
	{
		var ip = NetworkClientAdapter.Instance.IpAddress;
		var port = NetworkClientAdapter.Instance.Port;
		
		// Switch to Lobby Menu when a connection has been established to the selected Game-Server
		Debug.Log($"Connection Established to Server {ip}:{port}");
            
		MainMenuManager.Instance.lobbyPort = ip;
		MainMenuManager.Instance.lobbyIP = port.ToString();
		MainMenuManager.Instance.lobbyName = "Dummy Lobby Name from Join";   
            
		MainMenuManager.Instance.ShowLobby();
		
		// Unsubscribe on connected
		NetworkClientAdapter.Instance.Connected -= OnConnected;  
		
		// Send Salutation Message for updating Lobby on Server
		Debug.Log("Sending Salutation Message");
		var msg = new PlayerSalutationMessage
		{
			PlayerName = "Your mum fucks my bandwidth",
			PlayerId = Guid.NewGuid().ToString(),
		};
		NetworkRouter.SendToServer(msg, MessageType.PlayerSalutation);
	}


	public void ResetJoinLobbyTMDs()
	{
		Debug.Log("Reset Join Lobby Panel");
		lobbyIPTMP.text = LobbyIpTmpStandardValue;
		lobbyPortTMP.text = LobbyPortTmpStandardValue;
		DisableCreateLobbyButton();
	}

	private void DisableCreateLobbyButton()
	{
		joinLobbyButton.interactable = false;
	}
}