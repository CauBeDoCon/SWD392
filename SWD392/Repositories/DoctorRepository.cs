using Microsoft.AspNetCore.Identity;
using SWD392.DB;
using SWD392.DTOs;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
    }
}
