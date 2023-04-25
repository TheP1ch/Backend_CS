

namespace Backend_CS.Models
{
    public class Request {
        public int id { get; set; }
        public int requestDataId { get; set; }
        public virtual RequestData requestData { get; set; }
        public int workGroupId { get; set; }
        public virtual WorkGroup workGroup { get; set; }

        public Request()
        {
        }
        
        public Request(int workGroupId, string name, int price, int? workerId, int priorityId, int statusNumber)
        {
            this.workGroupId = workGroupId;
            this.requestDataId = this.id;
            this.requestData = new RequestData
            {
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

