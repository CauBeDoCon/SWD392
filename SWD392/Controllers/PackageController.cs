using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;
        public PackageController(IPackageRepository packageRepository)
           
        {
            _packageRepository = packageRepository;
           
        }
        [HttpGet("GetAllPackages")]
        public async Task<IActionResult> GetAllPackages()
        {
            var packages = await _packageRepository.GetAllPackagesAsync();
            return Ok(packages);
        }

        [HttpGet("GetPackage/{packageId}")]
        public async Task<IActionResult> GetPackage(int packageId)
        {
            var package = await _packageRepository.GetPackageByIdAsync(packageId);
            if (package == null)
            {
                return NotFound(new { Message = "Không tìm thấy gói điều trị." });
            }
            return Ok(package);
        }

        [HttpPost("CreatePackage")]
        public async Task<IActionResult> CreatePackage([FromBody] PackageDTO packageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newPackage = await _packageRepository.CreatePackageAsync(packageDto);
            return CreatedAtAction(nameof(GetPackage), new { packageId = newPackage.Id }, newPackage);
        }

        [HttpPut("UpdatePackage/{packageId}")]
        public async Task<IActionResult> UpdatePackage(int packageId, [FromBody] PackageDTO packageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _packageRepository.UpdatePackageAsync(packageId, packageDto);
            if (!success)
            {
                return BadRequest(new { Message = "Không thể cập nhật gói điều trị." });
            }
            return Ok(new { Message = "Gói điều trị đã được cập nhật." });
        }

        [HttpDelete("DeletePackage/{packageId}")]
        public async Task<IActionResult> DeletePackage(int packageId)
        {
            var success = await _packageRepository.DeletePackageAsync(packageId);
            if (!success)
            {
                return BadRequest(new { Message = "Không thể xóa Package. Hãy kiểm tra lại các ràng buộc dữ liệu." });
            }
            return Ok(new { Message = "Package đã được xóa thành công cùng với tất cả dữ liệu liên quan." });
        }


        [HttpPut("UpdatePackageSession")]
        public async Task<IActionResult> UpdatePackageSession([FromBody] PackageSessionDTO packageSessionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _packageRepository.UpdatePackageSessionAsync(packageSessionDto);
            if (!success)
            {
                return BadRequest(new { Message = "Không thể cập nhật PackageSession. Kiểm tra PackageId." });
            }

            return Ok(new { Message = "PackageSession đã được cập nhật thành công." });
        }
    }
}
