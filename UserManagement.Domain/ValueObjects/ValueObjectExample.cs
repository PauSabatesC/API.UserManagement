using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.ValueObjects
{
    class ValueObjectExample : ValueObject
    {
        public string x { get; set; }
        public string y { get; set; }
        public bool z { get; set; }
        public IEnumerable<string> w { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return x;
            yield return y;
            yield return z;
            yield return w;
        }
    }
}
