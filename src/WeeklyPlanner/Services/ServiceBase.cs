using System;
using WeeklyPlanner.Data;

namespace WeeklyPlanner.Services
{
    public abstract class ServiceBase
    {
        protected AppDbContext Context { get; }

        protected ServiceBase(AppDbContext context)
        {
            this.Context = context;
        }
        
    }
}