using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Leguar.TotalJSON;


namespace Game.DataPersistence
{
    public class JsonDataService : IDataService
    {
        private const string Key = "ggdPhkeOoiv6YMiPWa34kIuOdDUL7NwQFg6l1DVdwN8=";
        private const string Iv = "JZuM0HQsWSBVpRHTeRZMYQ==";

        public bool SaveData<T>(string relativePath, T data, bool encrypted)
        {
            string path = Application.persistentDataPath + relativePath;

            try
            {
                if (File.Exists(path))
            {
                Debug.Log("Data exists. Deleting old file and writing a new one!");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Writing file for the first time!");
            }
            using FileStream stream = File.Create(path);
            if (encrypted)
            {
                WriteEncryptedData(data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(path, JSON.Serialize(data).CreatePrettyString());
            }
                return true;
            }

            catch(Exception exception)
            {
                Debug.LogError($"Unable to save data due to: {exception.Message}\n {exception.StackTrace}");
                return false;
            }

        }

        public T LoadData<T>(string relativePath, bool encrypted)
        {
            string path = Application.persistentDataPath + relativePath;

            try
            {
                T data;
                if(encrypted)
                {
                    data = ReadEncryptedData<T>(path);
                }
                else
                {
                    JSON json = JSON.ParseString(File.ReadAllText(path));
                    data = json.Deserialize<T>();
                }

                return data;
            }
            catch(Exception exception)
            {
                Debug.LogError($"Failed to load data due to: {exception.Message}\n{exception.StackTrace}");
                throw exception;
            }
        }

        private void WriteEncryptedData<T>(T data, FileStream stream)
        {
            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(Key);
            aes.IV = Convert.FromBase64String(Iv);

            using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
            using CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);
            
            //Debug.Log($"Key: {Convert.ToBase64String(aes.Key)}");
            //Debug.Log($"Initialization Vector: {Convert.ToBase64String(aes.IV)}");
            
            cryptoStream.Write(Encoding.ASCII.GetBytes(JSON.Serialize(data).CreatePrettyString()));
        }

        private T ReadEncryptedData<T>(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            using Aes aes = Aes.Create();

            aes.Key = Convert.FromBase64String(Key);
            aes.IV = Convert.FromBase64String(Iv);

            using ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream decryptionStream = new MemoryStream(fileBytes);

            using CryptoStream cryptoStream = new CryptoStream(decryptionStream, cryptoTransform, CryptoStreamMode.Read);

            using StreamReader reader = new StreamReader(cryptoStream);
            string result = reader.ReadToEnd();

            Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or iv): {result}");
            JSON json = JSON.ParseString(result);
            return json.Deserialize<T>();
        }                
    }
}
