using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using OnlineVotingSystemAPI.Models;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using OnlineVotingSystemAPI.DTOs;

namespace OnlineVotingSystemAPI.Repositories.Interfaces
{
    //All the methods related to admin
    public interface IAdminRepository
    {
        //Purpose: Retrieves a list of all users.This is essential for an admin to manage users effectively.
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();

        //Purpose: Retrieves a specific user by their ID. This allows the admin to view details of a particular user.
        Task<UserDTO> GetUserByIdAsync(int id);

        //Purpose: Creates a new user.This is crucial for an admin who can add new users to the system.
        Task<User> CreateUserAsync(User user);

        //Purpose: Updates an existing user’s information.Admins often need to modify user details, such as email or roles.
        Task<bool> UpdateUserAsync(User user);

        //Purpose: Deletes a user by ID.Admins should have the ability to remove users from the system.
        Task<bool> DeleteUserAsync(int id);

        // Role management
        Task<Role> GetRoleByNameAsync(string roleName);

        //Admin's own functionality
        Task<AdminDTO?> GetAdminProfileAsync(int adminId);
        //Task<bool> UpdateAdminProfileAsync(Admin admin);
        //Task<bool> ChangeAdminPasswordAsync(int adminId, string oldPassword, string newPassword);
        Task<Admin?> GetAdminByEmailAsync(string email); // Added for registration
        Task<Admin?> CreateAdminAsync(Admin admin, List<string> roleNames); // Added for creating admin


        // Voting Event Methods
        Task<VotingEvent> CreateVotingEventAsync(VotingEvent votingEvent);
        Task<VotingEvent> GetVotingEventByIdAsync(int id);
        Task<bool> UpdateVotingEventAsync(VotingEvent votingEvent);
        Task<bool> DeleteVotingEventAsync(int id);

        //Candidates related Functionality
        Task<IEnumerable<CandidateDTO>> GetAllCandidatesAsync(int votingEventID);
        Task<CandidateDTO> GetCandidateByIdAsync(int candidateId);
        Task<Candidate> CreateCandidateAsync(Candidate candidate);
        Task<bool> UpdateCandidateAsync(CandidateDTO candidateDto);
        Task<bool> DeleteCandidateAsync(int candidateId);
    }
}
