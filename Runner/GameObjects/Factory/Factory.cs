using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    //Author: Arturas Tatariskinas

    public abstract class Factory
    {
        public abstract GameObject Create(string type);
    }
}
