using System;
namespace Backend_CS.Models
{
    public interface IUser
    {
        int id { get; }
        string name { get; set; }
        string imgUrl { get; set; }
    }
}

