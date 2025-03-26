using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PackageRepository : IPackageRepository
{
    private readonly ApplicationDbContext _context;

    public PackageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Package>> GetAllPackagesAsync()
    {
        return await _context.Packages.ToListAsync();
    }

    public async Task<Package> GetPackageByIdAsync(int packageId)
    {
        return await _context.Packages.FindAsync(packageId);
    }

    public async Task<Package> CreatePackageAsync(PackageDTO packageDto)
    {
        
        if (packageDto.Sessions > 4)
        {
            throw new ArgumentException("Số buổi điều trị (Sessions) không được lớn hơn 4.");
        }

        var package = new Package
        {
            Name = packageDto.Name,
            Price = packageDto.Price,
            Sessions = packageDto.Sessions,
            DoctorId = packageDto.DoctorId,
            Status = (packageDto.PackageCount == 0) ? "NotAvailable"
        : packageDto.DoctorId != null ? "active" : "inactive",

            PackageCount = packageDto.PackageCount

        };

        _context.Packages.Add(package);
        await _context.SaveChangesAsync();

       
        var packageSession = new PackageSession
        {
            PackageId = package.Id,
            TimeSlot1 = "09:00-10:00",
            Description1 = "Mô tả buổi 1"
        };

        if (packageDto.Sessions >= 2)
        {
            packageSession.TimeSlot2 = "10:00-11:00";
            packageSession.Description2 = "Mô tả buổi 2";
        }
        if (packageDto.Sessions >= 3)
        {
            packageSession.TimeSlot3 = "14:00-15:00";
            packageSession.Description3 = "Mô tả buổi 3";
        }
        if (packageDto.Sessions == 4)
        {
            packageSession.TimeSlot4 = "15:00-16:00";
            packageSession.Description4 = "Mô tả buổi 4";
        }

        _context.PackageSessions.Add(packageSession);
        await _context.SaveChangesAsync();

        return package;
    }



    public async Task<bool> UpdatePackageAsync(int packageId, PackageDTO packageDto)
    {
        var package = await _context.Packages.FindAsync(packageId);
        if (package == null) return false;

        if (!string.IsNullOrEmpty(packageDto.Name)) package.Name = packageDto.Name;
        if (packageDto.Price != null) package.Price = packageDto.Price;
        if (packageDto.Sessions != null) package.Sessions = packageDto.Sessions;
        if (packageDto.DoctorId != null) package.DoctorId = packageDto.DoctorId;
        if (packageDto.PackageCount < 0)
            throw new ArgumentException("Số lượng gói (PackageCount) không hợp lệ.");

        package.PackageCount = packageDto.PackageCount;

        if (package.PackageCount == 0)
        {
            package.Status = "inactive";
        }
        else
        {
            package.Status = package.DoctorId != null ? "active" : "inactive";
        }



        _context.Packages.Update(package);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePackageSessionAsync(int packageId, PackageSessionDTO packageSessionDto)
    {
        var packageSession = await _context.PackageSessions.FirstOrDefaultAsync(ps => ps.PackageId == packageId);
        var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);

        if (packageSession == null || package == null)
        {
            return false;
        }

    
        packageSession.TimeSlot1 = packageSessionDto.TimeSlot1;
        packageSession.Description1 = packageSessionDto.Description1;

        if (package.Sessions >= 2)
        {
            packageSession.TimeSlot2 = packageSessionDto.TimeSlot2;
            packageSession.Description2 = packageSessionDto.Description2;
        }
        else
        {
            packageSession.TimeSlot2 = null;
            packageSession.Description2 = null;
        }

        if (package.Sessions >= 3)
        {
            packageSession.TimeSlot3 = packageSessionDto.TimeSlot3;
            packageSession.Description3 = packageSessionDto.Description3;
        }
        else
        {
            packageSession.TimeSlot3 = null;
            packageSession.Description3 = null;
        }

        if (package.Sessions == 4)
        {
            packageSession.TimeSlot4 = packageSessionDto.TimeSlot4;
            packageSession.Description4 = packageSessionDto.Description4;
        }
        else
        {
            packageSession.TimeSlot4 = null;
            packageSession.Description4 = null;
        }

        _context.PackageSessions.Update(packageSession);
        await _context.SaveChangesAsync();

        return true;
    }




    public async Task<bool> DeletePackageAsync(int packageId)
    {
        var package = await _context.Packages
            .Include(p => p.PackageSessions)
            .Include(p => p.Appointments)
            .FirstOrDefaultAsync(p => p.Id == packageId);

        if (package == null)
        {
            return false;
        }

        
        if (package.PackageSessions.Any())
        {
            _context.PackageSessions.RemoveRange(package.PackageSessions);
        }

   
        var appointments = await _context.Appointments
            .Where(a => a.PackageId == packageId)
            .Include(a => a.TreatmentSessions)
            .ToListAsync();

        foreach (var appointment in appointments)
        {
         
            var treatmentSessionIds = appointment.TreatmentSessions.Select(ts => ts.Id).ToList();
            var packageTrackings = await _context.PackageTrackings
                .Where(pt => treatmentSessionIds.Contains(pt.TreatmentSessionId))
                .ToListAsync();

            if (packageTrackings.Any())
            {
                _context.PackageTrackings.RemoveRange(packageTrackings);
            }

            
            if (appointment.TreatmentSessions.Any())
            {
                _context.TreatmentSessions.RemoveRange(appointment.TreatmentSessions);
            }

           
            var users = await _context.Users
                .Where(u => u.AppointmentId == appointment.Id)
                .ToListAsync();

            foreach (var user in users)
            {
                user.AppointmentId = null;
            }

          
            _context.Appointments.Remove(appointment);
        }

    
        _context.Packages.Remove(package);

      
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<PackageSession>> GetAllPackageSessionsAsync()
    {
        return await _context.PackageSessions.ToListAsync();
    }

    
    public async Task<List<PackageSession>> GetPackageSessionsByPackageIdAsync(int packageId)
    {
        return await _context.PackageSessions
            .Where(ps => ps.PackageId == packageId)
            .ToListAsync();
    }

    public async Task<List<PackageWithDoctorDTO>> GetAllPackagesWithDoctorAsync()
    {
        var packages = await _context.Packages
            .Include(p => p.Doctor) 
            .ToListAsync();

        var result = packages.Select(p => new PackageWithDoctorDTO
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Sessions = p.Sessions,
            PackageCount = p.PackageCount,
            Status = p.Status,
            DoctorId = p.DoctorId,
            DoctorName = p.Doctor != null ? p.Doctor.FirstName + " " + p.Doctor.LastName : "Chưa gán bác sĩ",
            DoctorAvatar = p.Doctor?.Avatar ?? ""
        }).ToList();

        return result;
    }

}
