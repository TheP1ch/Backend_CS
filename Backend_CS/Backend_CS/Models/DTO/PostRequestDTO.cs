using System;
namespace Backend_CS.Models.DTO
{
	public class PostRequestDTO
	{
        public int workGroupId { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int? userId { get; set; }
        public int priorityId { get; set; }
        public int statusNumber { get; set; }
        public int statusPosition { get; set; }
        public string? comment { get; set; }
    }
}

