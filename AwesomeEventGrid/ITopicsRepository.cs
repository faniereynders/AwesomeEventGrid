using AwesomeEventGrid.Entities;
using System.Collections.Generic;

namespace AwesomeEventGrid
{
    public interface ITopicsRepository
    {
        Topic FindByName(string name);
        IEnumerable<Topic> GetAll();
        Topic Add(Topic topic);
    }
}