

namespace Backend_CS.Models
{
    public class Request {
        public int id { get; set; }
        public RequestData requestData { get; set; }
        public int workGroupId { get; set; }

        public Request()
        {
        }
        
        public Request(int workGroupId,int id, string name, int price, int? workerId, int priorityId, int statusNumber)
        {
            this.id = id;
            this.workGroupId = workGroupId;
            this.requestData = new RequestData
            {
                id = id,
                name = name,
                price = price,
                lastUpdateDate = DateTime.Now,
                createDate = DateTime.Now,
                userId = workerId,
                priorityId = priorityId,
                statusNumber = statusNumber,
                statusPosition = 0,
                comment = null
            };
        }

        public void changeUser(int userId)
        {
            if (requestData.userId != userId && userId != 0)
            {
                requestData.userId = userId;
            }
        }
    }
}

