using Riptide;

namespace Assets.Scripts.Network.Messages
{
	/// <summary>
	/// Provides utility methods for sending and broadcasting network messages
	/// within the application using the Riptide networking library.
	/// Derived classes can implement specific messaging logic.
	/// </summary>
	public class NetworkRouter
	{
		/// <summary>
		/// Creates a new message instance by serializing the given message and associating
		/// it with the specified message type for network communication.
		/// </summary>
		/// <param name="message">An object implementing the <c>IMessageSerializable</c> interface, containing the data to be serialized into the message.</param>
		/// <param name="messageType">The type of the message to be sent, represented by the <c>MessageType</c> enum.</param>
		/// <returns>Returns a new <c>Message</c> instance containing the serialized data and message type.</returns>
		private static Message CreateMessage(IMessageSerializable message, MessageType messageType)
		{
			var msgInstance = Message.Create(MessageSendMode.Reliable, (ushort)messageType);
			msgInstance.AddSerializable(message);
			return msgInstance;
		}

		/// <summary>
		/// Sends a network message to the server by serializing the provided
		/// <c>IMessageSerializable</c> object and associating it with the specified
		/// message type.
		/// </summary>
		/// <param name="message">An object implementing the <c>IMessageSerializable</c> interface, containing the data to be serialized into a network message.</param>
		/// <param name="messageType">The type of the message to be sent, identified by the <c>MessageType</c> enum.</param>
		public static void SendToServer(IMessageSerializable message, MessageType messageType)
		{
			var msg = CreateMessage(message, messageType);
			NetworkClientAdapter.Instance.SendMessage(msg);
		}

		/// <summary>
		/// Broadcasts a message to all connected clients in the server.
		/// </summary>
		/// <param name="message">An object implementing the <c>IMessageSerializable</c> interface, representing the data to be serialized and sent to all clients.</param>
		/// <param name="messageType">The type of the message being broadcasted, represented by the <c>MessageType</c> enum.</param>
		public static void Broadcast(IMessageSerializable message, MessageType messageType)
		{
			var msg = CreateMessage(message, messageType);
			NetworkServerAdapter.Instance.Server.SendToAll(msg);
		} 
	}
}