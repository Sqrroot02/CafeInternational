using System;
using Riptide;
using UnityEngine;

namespace Assets.Scripts.Network.Messages
{
    /// <summary>
    /// Represents a message that contains a salutation from a player, including their PlayerName.
    /// </summary>
    public class PlayerSalutationMessage : IMessageSerializable
    {
        [SerializeField]
        public string PlayerName { get; set; }

        public void Serialize(Message message)
        {
            message.AddString(PlayerName);
        }

        public void Deserialize(Message message)
        {
            message.GetString();
        }
    }
}
