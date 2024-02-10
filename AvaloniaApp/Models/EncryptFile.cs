using System;
using System.IO;
using System.Text;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Security.Cryptography;
using Avalonia.Controls.ApplicationLifetimes;

namespace AvaloniaApp.Models
{
    public class EncryptFile
    {
        public void Encrypt(string inputFile, string outputFile, string password)
        {
            try
            {
                if (password.Length != 8)
                {
                    throw new ArgumentException("Password length not equals 8!");
                }
                UnicodeEncoding UE = new();
                byte[] key = UE.GetBytes(password);

                var cryptFile = outputFile;
                FileStream fsCrypt = new(cryptFile, FileMode.Create);

                Aes RMCrypto = Aes.Create();

                CryptoStream cs = new(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Error!", "Encryption failed!", ButtonEnum.Ok);
                if (Avalonia.Application.Current.ApplicationLifetime is
                    IClassicDesktopStyleApplicationLifetime des)
                {
                    box.ShowWindowDialogAsync(des.MainWindow);
                }

            }
        }

        // дешифрование AES, оно же Рэндал, длинна кприкто ключа 8 букв
        public void Decrypt(string inputFile, string outputFile, string password)
        {
            UnicodeEncoding UE = new();
            byte[] key = UE.GetBytes(password);

            FileStream fsCrypt = new(inputFile, FileMode.Open);

            Aes RMCrypto = Aes.Create();

            CryptoStream cs = new(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

            FileStream fsOut = new(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();
        }
    }
}
