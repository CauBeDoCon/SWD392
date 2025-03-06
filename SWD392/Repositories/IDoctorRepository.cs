using SWD392.DTOs;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public interface IDoctorRepository
    {
        Task<bool> AddDoctorAsync(SignUpDTO doctorDto);
        Task<bool> UpdateDoctorAsync(string doctorId, UpdateDoctorDTO request);
        Task<bool> DeleteDoctorAsync(string doctorId);

        Task<List<DoctorDTO>> GetAllDoctorsAsync();

    }
}
