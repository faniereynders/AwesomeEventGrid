using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer1
{

    public class Subscription
    {
        public string Name
        {
            get; set;
        }
        public string EndpointUrl
        {
            get; set;
        }
        public string[] EventTypes
        {
            get; set;
        }
    }

}
