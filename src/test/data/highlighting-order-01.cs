using System.Collections.Generic;
using System;

namespace Test
{
    public class Foo
    {
        public virtual bool Bar(List<String> data)
        {
            return data.Count > 0;
        }
    }
}