using Application.Common.Protector;
using Application.Services.Booking;
using Application.Services.Country;
using Application.Services.Menu;
using Application.Services.Participant;
using Application.Services.Profile;
using Application.Services.Role;
using Application.Services.State;
using Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ProtectData>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ICountryServices, CountryServices>();
            services.AddScoped<IStateServices, StateServices>();
            services.AddScoped<IRoleServices,RoleServices>();
            services.AddScoped<IProfileServices, ProfileServices>();
            services.AddScoped<IMenuServices, MenuServices>();
            services.AddScoped<IBookingServices, BookingServices>();
            services.AddScoped<IParticipantServices, ParticipantServices>();
            return services;
        }
    }
}
