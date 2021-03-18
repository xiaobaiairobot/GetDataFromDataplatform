using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace GetDataFromDataplatform
{
    class Program
    {
        static void Main(string[] args)
        {
            //donet key 
            string privateKeyString = "<RSAKeyValue><Modulus>nWdTBVsU2WkVotNnch26G4ZpcqSjFxx/Nxrmp406q2C/LyAazbtkUhxo3UgBYvOGgL9eUIyYbxbhJJZm9GZSxQjofbut0ceRCXN2eErwqIAHw0gB0KR7ZUbWzevq/uIvT3vZMjf3Dme61pvi3SGmCE3wlkdMRphyUenn1m7VNsP/FuQwYxDUT7L7F9KlmiCt77bHZHMcfVJeIc8ChnOL9OOLNhjDohblvEQATGIZ4Ok8YfRMPKyRZsUOS9wbN7AfYE8+Y9ggjl9A9LoVWvtei//DHSVKmzlV3TFuF2PL15RQANHKn/GVfLfGMPN/JXIecgewcxMNJY54w2hZeqa7vw==</Modulus><Exponent>AQAB</Exponent><P>37giXt4eLev/RRatg7VHPeTNVdoagxhYb0TbD9HKy+VpK47xgv68i1wR2zT8vk6Q1bPhO+vB8QAOimTuk828pO5WVMXRI5pOPeMobXoAd1ffXftpYE3ChldBjasrDKhz35C0ONYbuveSGOk8mFU4yGsrv9mIov2TO2j+0SonIf0=</P><Q>tB2V5YeHfRwud8h/3K2SXzW++mLwgNYmPkPsalZerLf8xHYvTjZwW2WepBKi/F4EJxMV40ljB11BhJJMgePm1uiS9PazLz7uYS2acQJE9QR1LKmyTVf4aVlXNxQCScl0UgkXbnJUpAvzYYx2Gvh8w0gMZ8q3vFEe9r7D8Wbo02s=</Q><DP>R97hu9RjoaEVSt60M4HDN1EP3irJJaBmmKhoL7bYDZTPqpyrXcf0Tljvq5pkL8cRpHW7QALHNWMtSKyQMI+BsQKYXXeHboad6CfXzwPIro5eRdUEz1lryrixaI+6rsBXKVVVcIToN7JdDv+u5clLCqfTUDjUOnh4Gjfq1MtT8wk=</DP><DQ>kGVXkpfAqzA4oJcAyq3sNqAj5yACp4cHScuycN2lMNqfrfEBo7ZJBTzGncrDF4dX1OucFIb0+SZuLBYNd+R5X7k8d15/8FSHTyj8M7UaC0PznEa/RvpLkEmrfeBwpS74LfxT6JuH96wNBhtRY/XqL7RHHb6K5lIyfKYa+CEEbtk=</DQ><InverseQ>ik5UmWbZhYsAO85qDVQHzqaGBXsdG5l7xPyypbRyiGCSvK3yEOxHNn6fkA9Xk7IbCdqC+39L36erDBAgDdNfa8WdH4JBpXfdn0pt/fdozziFUysQuGNRoej6eiBzn9fgZoGwomlLcVxnrmgurHXBZs6JF5LoI7MK2A+PRAjOJKE=</InverseQ><D>NIJuoL/E0c6LqDkMLnaaSmpprRQdUC1yhwiTX9vucZOh0+/K4U0dZ4MQvJkz1TXWbgbpSMfOZmjkjNcb23ZzMEYLgT5l+zCvQs7g+7pMVHn5nPzdliP5Ak1ChYmpaIQ+Mi7nwygGZCjYnJV+djSudeDCXO/GE1rx8D+u1ss8l7r+GsyzOmRQbgqWOkFwOMOdau9ytpO0dZlLNZHcvU/K6gn+BrUAi7L4G9GvXc4WApgEbAj/x55FwHDmaO4asKBwwfpgRp3fZwj5ptom1N8vvxVIjkrusLsNDN2UyNfKDDQCJG5Yxe1WJdJQj1RLbFn9PmhvhEemNKJhb7UrBhwFaQ==</D></RSAKeyValue>";
            string password = getCipherText(privateKeyString);

            LoginBody body = new LoginBody()
            {
                //key id
                name = "2d68b8eb-a253-4dc2-a646-35e8d7502aad",
                password = password,
                type = "aksk"
            };

            string loginAddress = "http://172.30.13.177:30003/x-authorization-service/authorizations/logins";
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            byte[] responseData = client.UploadData(loginAddress, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body)));
            LoginResponseBody loginResponseBody = JsonConvert.DeserializeObject<LoginResponseBody>(Encoding.UTF8.GetString(responseData));
            if(loginResponseBody.code != 200)
            {
                //failed
            }

            string token = loginResponseBody.token;

            string executeAddress = "http://172.30.13.177:30003/x-gateway-service/gateways/sqls/execute";

            SqlExecuteBody executeBody = new SqlExecuteBody()
            {
                engine = "mpp",
                isIncludeHeaders = true,
                sql = "select * from eee limit 10",
                timeout = 15000
            };

            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("x-token", token);
            responseData = client.UploadData(executeAddress, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(executeBody)));

            string result = Encoding.UTF8.GetString(responseData);
            System.Console.Write(result);

            System.Console.Read();
        }

        private static string getCipherText(string privateKeyString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            rsa.FromXmlString(privateKeyString);
            string input = Guid.NewGuid().ToString() + "@" + (3600 * 24).ToString();
            byte[] result = rsa.SignData(Encoding.UTF8.GetBytes(input), new SHA256Managed());
            return input + "#" + System.Convert.ToBase64String(result);
        }


    }
}
