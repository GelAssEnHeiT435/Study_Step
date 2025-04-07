using Study_Step.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Study_Step.Services
{
    public class SecureTokenStorage : ITokenStorage
    {
        private static readonly string TokenFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudyStep", "token.dat");

        public void SaveRefreshToken(string token)
        {
            try
            {
                var directory = Path.GetDirectoryName(TokenFilePath);
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                byte[] encryptedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(token), null,
                                                              DataProtectionScope.CurrentUser);
                File.WriteAllBytes(TokenFilePath, encryptedBytes);
            }
            catch (Exception ex) {
                MessageBox.Show($"Ошибка сохранения токена: {ex.Message}");
            }
        }

        public string? LoadRefreshToken()
        {
            if (!File.Exists(TokenFilePath)) return null;

            try
            {
                byte[] encryptedBytes = File.ReadAllBytes(TokenFilePath);
                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null,
                                                                DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch {
                return null;
            }
        }

        public void DeleteRefreshToken()
        {
            if (File.Exists(TokenFilePath))
            {
                File.Delete(TokenFilePath);
            }
        }
    }
}
