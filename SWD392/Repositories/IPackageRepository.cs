using SWD392.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPackageRepository
{
    Task<IEnumerable<Package>> GetAllPackagesAsync();
    Task<Package> GetPackageByIdAsync(int packageId);
    Task<Package> CreatePackageAsync(PackageDTO packageDto);
    Task<bool> UpdatePackageAsync(int packageId, PackageDTO packageDto);
    Task<bool> DeletePackageAsync(int packageId);
    Task<bool> UpdatePackageSessionAsync(PackageSessionDTO packageSessionDto);

}
