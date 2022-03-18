using System;
using System.Runtime.Serialization;

namespace Messages.Models
{
    /// <summary>
    ///     Класс сообщений.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Messages
    {
        /// <summary>
        ///     Конструктор класса.
        /// </summary>
        /// <param name="subject">Заголовок сообщения.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="senderId">Email-отправителя.</param>
        /// <param name="receiverId">Email-получателя.</param>
        public Messages(string subject, string message, string senderId, string receiverId)
        {
            Subject = subject;
            Message = message;
            SenderId = senderId;
            ReceiverId = receiverId;
        }

        /// <summary>
        ///     Заголовок сообщения.
        /// </summary>
        /// <example>Something important..</example>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        ///     Сообщение пользователя.
        /// </summary>
        /// <example>Hi! I really love podbel, ya know!</example>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        ///     Email-отправителя.
        /// </summary>
        /// <example>ilovepodbel22@edu.hse.ru</example>
        [DataMember]
        public string SenderId { get; set; }

        /// <summary>
        ///     Email-получателя.
        /// </summary>
        /// <example>ilovepodbel22@edu.hse.ru</example>
        [DataMember]
        public string ReceiverId { get; set; }
    }
}