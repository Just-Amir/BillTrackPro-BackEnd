using System.ComponentModel.DataAnnotations;

namespace BillTrackPro.Domain.Entities;

public class UserProfile
{
    [Key]
    public int Id { get; set; }
    
    // Profile
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Timezone { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;

    // Business Info
    public string CompanyName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string CompanyLogoUrl { get; set; } = string.Empty;
    public string BrandColor { get; set; } = "#0F172A";
    public string SecondaryColor { get; set; } = "#F59E0B";
}
