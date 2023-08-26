using AutoMapper;
using todo_list.Entities;
using todo_list.Models;

namespace todo_list.Profiles;

public class UsersProfile : Profile
{
	public UsersProfile()
	{
		CreateMap<UserDto, User>();
	}
}
