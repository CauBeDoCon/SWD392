using Microsoft.EntityFrameworkCore;
using SWD392.DB;
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
            Status = packageDto.DoctorId != null ? "active" : "inactive",
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
        if (packageDto.PackageCount >= 0) package.PackageCount = packageDto.PackageCount;
        package.Status = package.DoctorId != null ? "active" : "inactive";

        _context.Packages.Update(package);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePackageSessionAsync(int packageId, PackageSessionDTO packageSessionDto)
    {
        var packageSession = await _context.PackageSessions.FirstOrDefaultAsync(ps => ps.PackageId == packageId);
        if (packageSession == null)
        {
            return false;
        }

        var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);
        if (package == null)
        {
            return false;
        }

        packageSession.TimeSlot1 = packageSessionDto.TimeSlot1;
        packageSession.Description1 = packageSessionDto.Description1;
        packageSession.TimeSlot2 = package.Sessions >= 2 ? packageSessionDto.TimeSlot2 : null;
        packageSession.Description2 = package.Sessions >= 2 ? packageSessionDto.Description2 : null;
        packageSession.TimeSlot3 = package.Sessions >= 3 ? packageSessionDto.TimeSlot3 : null;
        packageSession.Description3 = package.Sessions >= 3 ? packageSessionDto.Description3 : null;
        packageSession.TimeSlot4 = package.Sessions == 4 ? packageSessionDto.TimeSlot4 : null;
        packageSession.Description4 = package.Sessions == 4 ? packageSessionDto.Description4 : null;

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
}
