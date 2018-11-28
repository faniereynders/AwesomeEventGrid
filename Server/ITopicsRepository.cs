using System.Collections.Generic;

namespace WebApplication23
{
    public interface ITopicsRepository
    {
        Topic FindByName(string name);
        IEnumerable<Topic> GetAll();
        Topic Add(Topic topic);
    }
}