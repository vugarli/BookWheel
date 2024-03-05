using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Locations.Commands
{
    public class SetLocationCommandHandler
        : IRequestHandler<SetLocationCommand>
    {
        public SetLocationCommandHandler(ApplicationDbContext dbContext)
        {
            
        }

        public Task Handle(SetLocationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
