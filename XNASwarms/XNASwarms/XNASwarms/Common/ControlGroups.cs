using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms.Common
{
    public class ControlGroups : Dictionary<int, string>
    {
        public new void Add(int key, string value)
        {
            //Remove duplicates
            this.Remove(key);
            base.Add(key, value);
        }

        public new void Remove(int key)
        {
            base.Remove(key);
        }
    }
}
