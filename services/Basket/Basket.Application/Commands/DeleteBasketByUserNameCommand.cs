using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Commands
{
    public class DeleteBasketByUserNameCommand:IRequest<Unit>
    {
        public string UserName { get; set; }

        public DeleteBasketByUserNameCommand(string username)
        {
            UserName = username;
            
        }
    }
}
