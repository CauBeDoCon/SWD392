using System.ComponentModel.DataAnnotations;

public class PackageSessionDTO
{
    [Required]
    public int PackageId { get; set; }

    public string? TimeSlot1 { get; set; }
    public string? TimeSlot2 { get; set; }
    public string? TimeSlot3 { get; set; }
    public string? TimeSlot4 { get; set; }

    public string? Description1 { get; set; }
    public string? Description2 { get; set; }
    public string? Description3 { get; set; }
    public string? Description4 { get; set; }
}
