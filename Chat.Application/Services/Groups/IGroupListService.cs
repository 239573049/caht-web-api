using AutoMapper;
using Chat.Application.Dto.Groups;
using Chat.Core.Entities.Groups;
using Chat.Repository;
using Chat.Repository.Repositorys;
using Management.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Services.Groups
{
    public interface IGroupListService
    {
        Task<GroupListDto> CreateGroupListDtoAsync(GroupListDto dto);
    }
    public class GroupListService : BaseService<GroupList>, IGroupListService
    {
        private readonly IMapper _mapper;
        public GroupListService(
            IMapper mapper,
            IUnitOfWork<MasterDbContext> unitOfWork, 
            IMasterDbRepositoryBase<GroupList> currentRepository) 
            : base(unitOfWork, currentRepository)
        {
            _mapper = mapper;
        }

        public async Task<GroupListDto> CreateGroupListDtoAsync(GroupListDto dto)
        {
            var data=_mapper.Map<GroupList>(dto);
            data=await currentRepository.AddAsync(data);
            await unitOfWork.SaveChangesAsync();
            return _mapper.Map<GroupListDto>(data);
        }
    }
}
