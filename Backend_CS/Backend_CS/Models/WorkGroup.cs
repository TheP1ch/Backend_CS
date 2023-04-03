

namespace Backend_CS.Models
{
    public class WorkGroup {
        public int id { get; set; }
        public string name { get; set; }
        public List<RequestData> requestsData {get; set;}
        
        public WorkGroup() : this(0, "", new List<RequestData>())
        {
        }

        public WorkGroup(int id, string name, List<RequestData> requestsData)
        {
            this.id = id;
            this.name = name;
            this.requestsData = requestsData;
        }
    }
}

