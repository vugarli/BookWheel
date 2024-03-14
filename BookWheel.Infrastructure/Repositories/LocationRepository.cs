﻿using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Repositories;
using BookWheel.Domain.Specifications;
using BookWheel.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private ApplicationDbContext _applicationDbContext { get; }
        public IQueryable<Location> Queryable { get => _applicationDbContext.Set<Location>(); }
        public LocationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Location?> GetLocationBySpecificationAsync
            (
            Specification<Location> spec
            )
        => await Queryable.ApplySpecification(spec).FirstOrDefaultAsync();


        public void Update(Location location)
        {
            _applicationDbContext.Update(location);
        }

        public async Task AddAsync(Location location)
        {
            await _applicationDbContext.AddAsync(location);
        }


    }
}