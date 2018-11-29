using System;

namespace AwesomeEventGrid.Entities
{
    public class Topic
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();


        public string Name { get; set; }
    }
}