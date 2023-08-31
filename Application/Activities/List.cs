using Application.Core;
using Application.Interfaces;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ActivityDTO>>>
        {
            public ActivityParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDTO>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            public async Task<Result<PagedList<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Activities
                    .Where(d => d.Date >= request.Params.StartDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() })
                    .AsQueryable();

                //somente para o host | user logado
                if (request.Params.IsGoing)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.UserName == _userAccessor.GetUserName()));
                }

                if (request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.HostUserName == _userAccessor.GetUserName());
                }

                return Result<PagedList<ActivityDTO>>.Success(await PagedList<ActivityDTO>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
               );

                //Projecting loading - extensions from AutoMapper || We should use Select from LINQ
                /*
                var activities = await _context.Activities
                   //Projecting loading - extensions from AutoMapper || We should use Select from LINQ
                   .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() })
                   .ToListAsync(cancellationToken);

                //Eagle loading
                .Include(a => a.Attendees)
                .ThenInclude(u => u.AppUser)
                .ToListAsync(cancellationToken);
                */

                //return Result<List<Activity>>.Success(await _context.Activities.ToListAsync());

                //Eagle loading
                /*
                .Include(a => a.Attendees)
                .ThenInclude(u => u.AppUser)
                .ToListAsync(cancellationToken);
                */

                //var activitiesResult = _mapper.Map<List<ActivityDTO>>(activities);
            }
        }
    }
}
