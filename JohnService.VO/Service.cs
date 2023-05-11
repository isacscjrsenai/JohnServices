using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnService.VO
{
    public class Services : Dictionary<string, string>
    {
        public Services(Dictionary<string, string> dict)
        {
            foreach (var item in dict)
            {
                this.Add(item.Key, item.Value);
            }

        }
    }
}
