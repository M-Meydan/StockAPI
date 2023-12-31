﻿using AutoMapper;
using System;
using System.Linq;
using System.Reflection;
using WebAPI.App.Trades.Commands.CreateTrade;
using WebAPI.Domain.Entities;

namespace WebAPI.App.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        AddCustomMappings();
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void AddCustomMappings()
    {
        CreateMap<CreateTradeCommand, Trade>();
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))).ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { this });

        }
    }
}
