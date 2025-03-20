using System.ComponentModel.DataAnnotations;

public class PackageDTO
{
    
    [Required(ErrorMessage = "Tên gói điều trị là bắt buộc khi tạo mới.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Giá gói điều trị là bắt buộc khi tạo mới.")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Số buổi là bắt buộc khi tạo mới.")]
    [Range(1, 4, ErrorMessage = "Số buổi phải từ 1 đến 4.")]
    public int Sessions { get; set; }
    [Required(ErrorMessage = "Số lượng gói còn lại là bắt buộc.")]
    [Range(1, 5, ErrorMessage = "Số lượng gói phải từ 1 -> 5.")]
    public int PackageCount { get; set; }
    [Required(ErrorMessage = "Tên bác sĩ phụ trách là bắt buộc khi tạo mới.")]
    public string? DoctorId { get; set; }
}
