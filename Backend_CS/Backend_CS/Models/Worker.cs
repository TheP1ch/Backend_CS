using System;


namespace Backend_CS.Models
{
    

	public class Worker : IUser
	{
        public int id { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }
        public bool isAdmin { get; set; }

        //default constructor

        public Worker() : this(0)
        {

        }

        public Worker(int id) : this(id, "", "", false)
        {
        }

        public Worker(int id, string name, string imgUrl, bool isAdmin)
        {
            this.id = id;
            this.name = name;
            this.imgUrl = imgUrl;
            this.isAdmin = isAdmin;
        }

        public List<RequestData> GetWorkerRequests(List<Request> requests)
        {
            return requests
                .Where(r => r.requestData.userId == id)
                .Select(r => r.requestData)
                .ToList();
        }

    }
}

