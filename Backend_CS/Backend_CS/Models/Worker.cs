using System;
using NuGet.Protocol.Plugins;
using System.Text;
using System.Security.Cryptography;


namespace Backend_CS.Models
{
    

	public class Worker : IUser
	{
        public int id { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }
        private byte[] Password { get; set; }
        public string PasswordHash
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var b in MD5.HashData(Password))
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
            set { Password = Encoding.UTF8.GetBytes(value); }
        }

        public bool IsAdmin => name == "admin";

        public bool CheckPassword(string password) => PasswordHash == password;

        //default constructor

        public Worker()
        {

        }

        public Worker(string name, string imgUrl, string password)
        {
            this.name = name;
            this.imgUrl = imgUrl;
            this.PasswordHash = password;
        }

    }
}

