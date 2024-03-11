using AutoMapper;
using BookWheel.Application.Schedules.Commands.Create;
using BookWheel.Domain.LocationAggregate;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Profiles
{
    public class ScheduleProfile
        : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<CreateScheduleCommand, Schedule>()
                .ConstructUsing(opt=> new Schedule(opt.LocationId,opt.ScheduleDate,new Domain.Value_Objects.SchedulePrice() {Amount = opt.Amount }));
        }
    }
}
