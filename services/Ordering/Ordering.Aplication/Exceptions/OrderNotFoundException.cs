using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException(string name , int id):Exception($"Entity {name}-{id} is not found ")
    {
    }
}
