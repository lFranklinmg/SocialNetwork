﻿using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                    //Eagle loading
                    .Include(a => a.Attendees)
                    .ThenInclude(u => u.AppUser)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);
                    //.FirstOrDefaultAsync(x => x.Id == request.Id); *Nao retorna excessao caso haja mais 1!

                if (activity == null) return null;

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

                if (user == null) return null;

                //Busca o Host da Actvity - * in Memory pq já temos a carga no activity object
                var hostUserName = activity.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName;

                var attendance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                //Modifica Status da Atividade * Somente o Host pode fazer!
                if (attendance != null && hostUserName == user.UserName)
                    activity.IsCancelled = !activity.IsCancelled;

                //Remove o Attendee da Atividade ou o Add
                if (attendance != null && hostUserName != user.UserName)
                    activity.Attendees.Remove(attendance);

                if (attendance == null)
                {
                    attendance = new ActivityAttendee
                    {
                        Activity = activity,
                        AppUser = user,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Fail during Updating Attendance");

            }
                    
        }
    }
}
