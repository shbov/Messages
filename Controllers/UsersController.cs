using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Faker;
using Messages.Models;
using Microsoft.AspNetCore.Mvc;

namespace Messages.Controllers
{
    /// <summary>
    ///     Класс, отвечающий за работу с пользователями
    /// </summary>
    [Route("users")]
    public class UsersController : Controller
    {
        private static readonly Random Rnd = new();

        private List<Models.Messages> _messages = new();

        private List<Users> _users = new();

        /// <summary>
        ///     Заполнить рандомными данными.
        /// </summary>
        /// <returns></returns>
        [HttpPost("fill")]
        public IActionResult Fill()
        {
            _users = new List<Users>();
            _messages = new List<Models.Messages>();

            var count = Rnd.Next(3, 15);
            for (var i = 0; i < count; i++)
                _users.Add(new Users(GenerateUserName(), GenerateEmail()));

            var messageCount = Rnd.Next(4, 20);
            for (var i = 0; i < messageCount; i++)
            {
                var (human1, human2) = GetTwoPeopleFromList();

                _messages.Add(new Models.Messages(Lorem.Sentence(2), Lorem.Paragraph(1), human1, human2));
            }

            SaveData();
            return Ok();
        }

        /// <summary>
        ///     Создать нового пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="email">Почта.</param>
        /// <returns></returns>
        [HttpPost("/users/{userName}/{email}")]
        public IActionResult CreateNewUser(string userName, string email)
        {
            LoadData();
            var user = new Users(userName, email);
            if (_users.Any(item => item.Email == email)) // || item.UserName == userName
                return BadRequest("Пользователь с такими данными уже существует.");

            _users.Add(user);
            SaveData();

            return Ok();
        }

        /// <summary>
        ///     Получить список всех пользователей.
        /// </summary>
        /// <returns>json-объект со всеми пользователями.</returns>
        [HttpGet("/users")]
        public IActionResult GetAllUsers()
        {
            LoadData();

            return Ok(_users);
        }

        /// <summary>
        ///     Получить информацию о пользователе по его почте.
        /// </summary>
        /// <param name="email">Почта.</param>
        /// <returns>json-объект о пользователе.</returns>
        [HttpGet("{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            LoadData();

            var user = _users.FirstOrDefault(item => item.Email == email);
            if (user != null)
                return Ok(user);

            return NotFound();
        }

        /// <summary>
        ///     Сохранить все данные в json.
        /// </summary>
        private void SaveData()
        {
            try
            {
                _users.Sort((a, b) => string.Compare(a.Email, b.Email, StringComparison.Ordinal));

                var users = JsonSerializer.Serialize(_users);
                var messages = JsonSerializer.Serialize(_messages);

                Console.WriteLine("start saving...");

                using (var fs = new FileStream("users.json", FileMode.Create, FileAccess.Write))
                {
                    using var sw = new StreamWriter(fs, Encoding.Default);
                    sw.Write(users);
                }

                Console.WriteLine("users done.");

                using (var fs = new FileStream("messages.json", FileMode.Create, FileAccess.Write))
                {
                    using var sw = new StreamWriter(fs, Encoding.Default);
                    sw.Write(messages);
                }

                Console.WriteLine("messages done.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        ///     Загрузить данные из json.
        /// </summary>
        private void LoadData()
        {
            _users = new List<Users>();
            _messages = new List<Models.Messages>();

            Console.WriteLine("start loading data...");
            try
            {
                var users = System.IO.File.ReadAllText("users.json", Encoding.Default);
                var messages = System.IO.File.ReadAllText("messages.json", Encoding.Default);

                _users = JsonSerializer.Deserialize<List<Users>>(users);
                _messages = JsonSerializer.Deserialize<List<Models.Messages>>(messages);

                Console.WriteLine("data loaded.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        ///     Получить два уникальных email'a из всех пользователей.
        /// </summary>
        /// <returns>(email, email).</returns>
        private (string, string) GetTwoPeopleFromList()
        {
            var people = _users.Count;
            var human1 = _users[Rnd.Next(0, people)];
            var human2 = _users[Rnd.Next(0, people)];

            while (human1.Email == human2.Email)
            {
                human1 = _users[Rnd.Next(0, people)];
                human2 = _users[Rnd.Next(0, people)];
            }

            return (human1.Email, human2.Email);
        }

        /// <summary>
        ///     Создать уникальную почту.
        /// </summary>
        /// <returns>Почта.</returns>
        private string GenerateEmail()
        {
            var email = Internet.Email();

            while (_users.FirstOrDefault(item => item.Email == email) != null)
                email = Internet.Email();

            return email;
        }

        /// <summary>
        ///     Создать уникальное имя.
        /// </summary>
        /// <returns>Имя.</returns>
        private string GenerateUserName()
        {
            var userName = Internet.UserName();

            // В условии не требуется уникальность имени, но я все же решил добавить сделать:
            while (_users.FirstOrDefault(item => item.Email == userName) != null)
                userName = Internet.UserName();

            return userName;
        }
    }
}