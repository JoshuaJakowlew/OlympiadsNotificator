using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Olympiads
{
    class OlympiadComparer : IEqualityComparer<Olympiad>
    {
        string ComparingProperty { get; set; }
        public OlympiadComparer(string property)
        {
            ComparingProperty = property;
        }
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Olympiad x, Olympiad y)
        {

            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (x is null || y is null)
                return false;

            //Check whether the products' properties are equal.
            return typeof(Olympiad).GetProperty(ComparingProperty).GetValue(x)
                .Equals(typeof(Olympiad).GetProperty(ComparingProperty).GetValue(y));
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Olympiad olympiad)
        {
            //Check whether the object is null
            if (olympiad is null) return 0;

            //Get hash code for the Name field if it is not null.
            int hashOlympiadProp = 
                typeof(Olympiad).GetProperty(ComparingProperty).GetValue(olympiad) == null ?
                0 :
                typeof(Olympiad).GetProperty(ComparingProperty).GetValue(olympiad).GetHashCode();

            //Calculate the hash code for the product.
            return hashOlympiadProp;
        }

    }
}
