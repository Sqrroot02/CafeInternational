using Assets.Scripts.Models;
using Assets.Scripts.Network.Messages;
using Riptide;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyPanelManager : MonoBehaviour
{
	public static LobbyPanelManager Instance;
	public TMP_Text lobbyNameTMP;

	public TMP_Text lobbyPortTMP;

	public TMP_Text lobbyIPTMP;

	public VerticalLayoutGroup PlayerListGroup;
	public GameObject LobbyPlayerItem;
	private Player[] _playerList;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}
    
	public void initiateLobby()
	{
		SetLobbyName();
		SetLobbyPortIPTMP();
	}

	private void SetLobbyName()
	{
		string lobbyName = MainMenuManager.Instance.lobbyName;
		Debug.Log($"Set Lobbyname to {lobbyName}" );
		lobbyNameTMP.text = "Lobby: " + lobbyName;
	}

	private void SetLobbyPortIPTMP()
	{
		var lobbyIP = MainMenuManager.Instance.lobbyIP;
		var lobbyPort = MainMenuManager.Instance.lobbyPort;

		if (!string.IsNullOrEmpty(lobbyPort))
		{
			lobbyPortTMP.text = "Port: " + lobbyPort;

		}

		Debug.Log("Set Lobby Port: " + lobbyPort + " Lobby IP: " + lobbyIP);
		lobbyIPTMP.text = "IP: " + lobbyIP;
	}

	public void ResetLobbyTMPs()
	{
		lobbyNameTMP.text = string.Empty;
		lobbyPortTMP.text = string.Empty;
		lobbyIPTMP.text = string.Empty;
	}

	/// <summary>
	/// Updates player list on lobby action notification has arrived
	/// </summary>
	/// <param name="message"></param>
	public void PlayerLobbyUpdate(PlayerLobbyActionMessage message)
	{
		lobbyNameTMP.text = message.LobbyName;
		MainMenuManager.Instance.lobbyName = message.LobbyName;

		_playerList = message.Players;
		
		// Remove all Labels
		for (var i = 0; i < PlayerListGroup.transform.childCount; i++)
			Destroy(PlayerListGroup.transform.GetChild(i).gameObject);
		
		// Construct the updated version of Players 
		foreach (var player in message.Players)
		{
			var item = Instantiate(LobbyPlayerItem, PlayerListGroup.transform);
			var playerNameComponent = item.transform.Find("PlayerNameLabel").GetComponent<TextMeshProUGUI>();
			playerNameComponent.text = player.PlayerName;	
		}
	}

	/// <summary>
	/// Sends a message to the server to initiate the game, specifying the lobby details,
	/// list of players, and randomly selecting a player as the game starter.
	/// </summary>
	public void StartGame()
	{
		var startGameMessage = new StartGameMessage
		{
			LobbyName = MainMenuManager.Instance.lobbyName,
			Players = _playerList,
			Starter = _playerList[Random.Range(0, _playerList.Length)]
		};
		NetworkRouter.SendToServer(startGameMessage, MessageType.StartGame);
	}
    
	/// <summary>
	/// Handles player actions
	/// </summary>
	/// <param name="message"></param>
	[MessageHandler(1001)]
	private static void HandlePlayerActions(Message message)
	{
		var obj = message.GetSerializable<PlayerLobbyActionMessage>();
		Instance.PlayerLobbyUpdate(obj);
	}
}