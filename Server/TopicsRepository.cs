using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication23
{
    public class TopicsRepository : ITopicsRepository
    {
        private readonly Data data;

        public TopicsRepository(Data data)
        {
            this.data = data;
        }

        public Topic Add(Topic topic)
        {
            data.Topics.Add(topic);
            //throw new NotImplementedException();
            return topic;
        }

        public Topic FindByName(string name)
        {
            return data.Topics.FirstOrDefault(t=>t.Name.Equals(name,StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<Topic> GetAll()
        {
            return data.Topics.ToList();
        }
    }
}
