using AutoMapper;
using onlineshop.Models;
using onlineshop.ViewModels;

namespace onlineshop.Helpers;

public static class MapperHelper
{
    public static UserViewModel ToViewModel(this MyUser entity)
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<UserViewModel, MyUser>()
               .ForAllMembers(opt =>
                   opt.Condition((src, dest, srcMember) => srcMember != null)
               )
        );
        var mapper = new Mapper(config);

        return mapper.Map<UserViewModel>(entity);
    }

    public static List<UserViewModel> ToViewModel(this List<MyUser> entities)
    {
        var viewModels = entities.Select(x => new UserViewModel
        {
            Id = x.Id,
            IsActive = x.IsActive,
            FirstName = x.FirstName,
            LastName = x.LastName,
            FullName = $"{x.FirstName} {x.LastName}",
            PhoneNumber = x.PhoneNumber
        }).ToList();

        return viewModels;
    }
}
