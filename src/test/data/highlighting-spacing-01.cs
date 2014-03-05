using System;

using System.Collections.Generic;

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