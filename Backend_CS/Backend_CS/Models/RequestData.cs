using System;
using Azure.Core;

namespace Backend_CS.Models
{
	public class RequestData
	{
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public DateTime lastUpdateDate { get; set; }
        public DateTime createDate { get; set; }
        public int? userId { get; set; }
        public int priorityId { get; set; }
        public int statusNumber { get; set; }
        public int statusPosition { get; set; }
        public string? comment { get; set; }
    }

    

}

