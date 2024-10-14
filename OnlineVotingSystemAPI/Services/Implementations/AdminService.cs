using OnlineVotingSystemAPI.DTOs;
using OnlineVotingSystemAPI.Models;
using OnlineVotingSystemAPI.Repositories.Interfaces;
using OnlineVotingSystemAPI.Services.Interfaces;

namespace OnlineVotingSystemAPI.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            return await _adminRepository.GetAllUsersAsync();
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            return await _adminRepository.GetUserByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await _adminRepository.CreateUserAsync(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _adminRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _adminRepository.DeleteUserAsync(id);
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _adminRepository.GetRoleByNameAsync(roleName);
        }

        public async Task<AdminDTO?> GetAdminProfileAsync(int adminId)
        {
            if (adminId <= 0)
            {
                throw new ArgumentException("Admin ID must be greater than zero.", nameof(adminId));
            }

            var profile = await _adminRepository.GetAdminProfileAsync(adminId);

            if (profile == null)
            {
                throw new KeyNotFoundException($"Admin with ID {adminId} not found.");
            }

            return profile;
        }

        //public async Task<bool> UpdateAdminProfileAsync(Admin admin)
        //{
        //    if (admin == null)
        //    {
        //        throw new ArgumentNullException(nameof(admin), "Admin cannot be null.");
        //    }

        //    return await _adminRepository.UpdateAdminProfileAsync(admin);
        //}

        //public async Task<bool> ChangeAdminPasswordAsync(int adminId, ChangePasswordDTO changePasswordDTO)
        //{
        //    if (changePasswordDTO == null)
        //    {
        //        throw new ArgumentNullException(nameof(changePasswordDTO), "Change password data cannot be null.");
        //    }

        //    if (string.IsNullOrWhiteSpace(changePasswordDTO.OldPassword) || string.IsNullOrWhiteSpace(changePasswordDTO.NewPassword))
        //    {
        //        throw new ArgumentException("Old and new passwords must be provided.");
        //    }

        //    return await _adminRepository.ChangeAdminPasswordAsync(adminId, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
        //}

        public async Task<Admin?> GetAdminByEmailAsync(string email)
        {
            return await _adminRepository.GetAdminByEmailAsync(email);
        }

        public async Task<Admin?> CreateAdminAsync(Admin admin, List<string> roleNames)
        {
            return await _adminRepository.CreateAdminAsync(admin, roleNames);
        }

        //Voting Event Mgmt
        public async Task<VotingEvent> CreateVotingEventAsync(VotingEvent votingEvent)
        {
            return await _adminRepository.CreateVotingEventAsync(votingEvent);
        }

        public async Task<bool> UpdateVotingEventAsync(VotingEvent votingEvent)
        {
            return await _adminRepository.UpdateVotingEventAsync(votingEvent);
        }

        public async Task<bool> DeleteVotingEventAsync(int id)
        {
            return await _adminRepository.DeleteVotingEventAsync(id);
        }

        public async Task<IEnumerable<CandidateDTO>> GetAllCandidatesAsync(int votingEventID)
        {
            return await _adminRepository.GetAllCandidatesAsync(votingEventID);
        }

        public async Task<CandidateDTO> GetCandidateByIdAsync(int candidateId)
        {
            return await _adminRepository.GetCandidateByIdAsync(candidateId);
        }

        public async Task<CandidateDTO> CreateCandidateAsync(CandidateDTO candidateDTO)
        {
            var candidate = new Candidate
            {
                Name = candidateDTO.Name,
                Party = candidateDTO.Party,
                VotingEventId = candidateDTO.VotingEventId
            };

            var createdCandidate = await _adminRepository.CreateCandidateAsync(candidate);

            return new CandidateDTO
            {
                Id = createdCandidate.Id,
                Name = createdCandidate.Name,
                Party = createdCandidate.Party,
                VotingEventId = createdCandidate.VotingEventId
            };
        }

        public async Task<bool> UpdateCandidateAsync(CandidateDTO candidateDTO)
        {
            //var candidate = new Candidate
            //{
            //    Id = candidateDTO.Id,
            //    Name = candidateDTO.Name,
            //    Party = candidateDTO.Party,
            //    VotingEventId = candidateDTO.VotingEventId
            //};

            return await _adminRepository.UpdateCandidateAsync(candidateDTO);
        }

        public async Task<bool> DeleteCandidateAsync(int candidateId)
        {
            return await _adminRepository.DeleteCandidateAsync(candidateId);
        }

        public async Task<VotingEvent> GetVotingEventByIdAsync(int id)
        {
            return await _adminRepository.GetVotingEventByIdAsync(id);
        }
    }
}
