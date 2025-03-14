using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DoctorRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> AddDoctorAsync(SignUpDTO doctorDto)
        {
            var doctor = new ApplicationUser
            {
                UserName = doctorDto.Username,
                Email = doctorDto.Email,
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName
            };

            var result = await _userManager.CreateAsync(doctor, doctorDto.Password);
            if (!result.Succeeded)
            {
                return false;
            }

            await _userManager.AddToRoleAsync(doctor, "Doctor");
            return true;
        }

        public async Task<bool> UpdateDoctorAsync(string doctorId, UpdateDoctorDTO request)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor == null) return false;

            doctor.FirstName = request.FirstName;
            doctor.LastName = request.LastName;

            var result = await _userManager.UpdateAsync(doctor);
            return result.Succeeded;
        }

        public async Task<bool> DeleteDoctorAsync(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor == null) return false;

            var result = await _userManager.DeleteAsync(doctor);
            return result.Succeeded;
        }

        public async Task<List<DoctorDTO>> GetAllDoctorsAsync()
        {
            return await _context.Users
            .Where(u => _context.UserRoles
                    .Where(ur => _context.Roles
                        .Where(r => r.Name == "Doctor")
                        .Select(r => r.Id)
                        .Contains(ur.RoleId))
                    .Select(ur => ur.UserId)
                    .Contains(u.Id))
                .Select(u => new DoctorDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Avatar = u.Avatar
                })
                .ToListAsync();
        }

    }
}
