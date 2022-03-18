using System;
using System.Runtime.Serialization;

namespace Messages.Models
{
    /// <summary>
    ///     Класс, отвечающий за модель пользователя.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Users
    {
        /// <summary>
        ///     Публичный конструктор.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="email">Email.</param>
        public Users(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        /// <summary>
        ///     Имя пользователя
        /// </summary>
        /// <example>ilovepodbel22</example>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        ///     Email пользователя.
        /// </summary>
        /// <example>ilovepodbel22@edu.hse.ru</example>
        [DataMember]
        public string Email { get; set; }
    }
}