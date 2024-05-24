using Ardalis.GuardClauses;
using BookWheel.Domain.Exceptions;
using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications.Location;
using Microsoft.Identity.Client;

namespace BookWheel.Domain.Services;

public class OwnerLocationSetter
{
    private readonly IUnitOfWork _unitOfWork;
    public IUserRepository UserRepository { get; set; }
    public ILocationRepository LocationRepository { get; set; }
    
    public OwnerLocationSetter
        (
            IUserRepository userRepository,
            ILocationRepository locationRepository,
            IUnitOfWork unitOfWork
        )
    {
        _unitOfWork = unitOfWork;
        UserRepository = userRepository;
        LocationRepository = locationRepository;
    }


    public async Task SetLocationToOwnerAsync(Guid ownerId, Location location,CancellationToken cancellationToken)
    {
        //validate owner exists
        if (ownerId != location.OwnerId)
            throw new Exception("Ids wont match");
        
        var spec = new GetLocationByOwnerSpecification(ownerId);
        
        if (await LocationRepository.CheckLocationBySpecificationAsync(spec))
        {
            throw new OwnerAlreadyHasLocationSet();
        }

        await LocationRepository.AddLocationAsync(location);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    
    
    
}