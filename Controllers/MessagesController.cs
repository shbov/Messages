using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Messages.Controllers
{
    /// <summary>
    ///     Класс, отвечающий за работу сообщений.
    /// </summary>
    [Route("messages")]
    public class MessagesController : Controller
    {
        private List<Models.Messages> _messages = new();

        /// <summary>
        ///     Получить все сообщения от {email}.
        /// </summary>
        /// <param name="email">Email-адрес.</param>
        /// <returns>json сообщений.</returns>
        [HttpGet("from/{email}")]
        public IActionResult GetMessagesFromUser(string email)
        {
            LoadData();

            var messages = _messages.Where(item => item.SenderId == email).ToList();
            if (messages.Count > 0)
                return Ok(messages);

            return NotFound();
        }

        /// <summary>
        ///     Получить все сообщения для {email}.
        /// </summary>
        /// <param name="email">Email-адрес.</param>
        /// <returns>json сообщений.</returns>
        [HttpGet("to/{email}")]
        public IActionResult GetMessagesToUser(string email)
        {
            LoadData();

            var messages = _messages.Where(item => item.ReceiverId == email).ToList();
            if (messages.Count > 0)
                return Ok(messages);

            return NotFound();
        }

        /// <summary>
        ///     Получить все сообщения от {from} для {to}.
        /// </summary>
        /// <param name="from">Email-адрес отправителя.</param>
        /// <param name="to">Email-адрес получателя.</param>
        /// <returns>json сообщений.</returns>
        [HttpGet("{from}/{to}")]
        public IActionResult GetMessagesByUser(string from, string to)
        {
            LoadData();

            var messages = _messages.Where(item => item.SenderId == from && item.ReceiverId == to).ToList();
            if (messages.Count > 0)
                return Ok(messages);

            return NotFound();
        }

        /// <summary>
        ///     Загрузить всю информацию из json.
        /// </summary>
        private void LoadData()
        {
            _messages = new List<Models.Messages>();

            try
            {
                var messages = System.IO.File.ReadAllText("messages.json", Encoding.Default);
                _messages = JsonSerializer.Deserialize<List<Models.Messages>>(messages);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}