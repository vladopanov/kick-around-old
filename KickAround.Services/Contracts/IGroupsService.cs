using System.Collections.Generic;
using KickAround.Data;
using KickAround.Models.BindingModels.Groups;
using KickAround.Models.EntityModels;
using KickAround.Models.ViewModels.Groups;

namespace KickAround.Services.Contracts
{
    public interface IGroupsService
    {
        IEnumerable<GroupViewModel> GetAllGroups(string userId);
        object GetGroupDetails(string groupdId, string userId);
        void CreateGroup(CreateGroupBindingModel bindingModel, string userId);
        object GetEditGroupModel(string id);
        void EditGroup(EditGroupBindingModel bindingModel);
        void DeleteGroup(string id);
        Group GetGroup(string id);
        bool IsUserAuthorizedToSeeGroupDetails(string userId, Group group);
        bool IsUserAuthorizedToModifyGroup(string userId, Group group);
        void JoinGroupRequest(string groupId, string userId);
        IEnumerable<Group> SearchGroups(string query);
        KickAroundContext Context { get; }
    }
}