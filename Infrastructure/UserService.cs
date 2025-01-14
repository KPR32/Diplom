﻿using FurnitureStore3.Domain.Entities;
using FurnitureStore3.Domain.Services;
using System.Security.Cryptography;
using System.Text;

namespace FurnitureStore3.Infrastructure
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> users;
        private readonly IRepository<Role> roles;

        public UserService(IRepository<User> usersRepository,
            IRepository<Role> rolesRepository)
        {
            users = usersRepository;
            roles = rolesRepository;
        }

        private string GetSalt() =>
   DateTime.UtcNow.ToString() + DateTime.Now.Ticks;

        private string GetSha256(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = SHA256.HashData(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public async Task<bool> IsUserExistsAsync(string username)
        {
            username = username.Trim();
            User? found = (await users.FindWhere(u => u.Login == username)).FirstOrDefault();
            return found is not null;
        }

        public async Task<User> RegistrationAsync(string fullname, string username, string phone, string password)
        {
            string salt()
            {
                using (Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes("password", 16)) // Генерация 128-битного ключа
                {
                    return Convert.ToBase64String(keyGenerator.GetBytes(16));
                }
            }
            string EncryptString(string plainText, string key)
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Convert.FromBase64String(key);
                    aesAlg.IV = new byte[16]; // Инициализационный вектор

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            string key = salt();
            // проверяем, есть ли пользователь с таким же username
            bool userExists = await IsUserExistsAsync(username);
            if (userExists) throw new ArgumentException("Username already exists");

            // находим роль "клиент"
            Role? clientRole = (await roles.FindWhere(r => r.Name == "client")).FirstOrDefault();

            if (clientRole is null)
                throw new InvalidOperationException("Role 'client' not found in database");

            // добавляем пользователя
            User toAdd = new User
            {
                Fullname = fullname,
                Login = username,
                Salt = GetSalt(),
                Phone = EncryptString(phone, key),
                //Phone = phone,
                Key = key,
                RoleId = clientRole.Id
            };
            toAdd.Password = GetSha256(password, toAdd.Salt);            
            return await users.AddAsync(toAdd);
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            username = username.Trim();
            User? user = (await users.FindWhere(u => u.Login == username)).FirstOrDefault();
            if (user is null)
            {
                return null;
            }
            string hashPassword = GetSha256(password, user.Salt);
            if (user.Password != hashPassword)
            {
                return null;
            }

            return user;
        }
    }
}
