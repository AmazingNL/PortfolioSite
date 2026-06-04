using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PortfolioSite.Services;

/// <summary>
/// Single-row table holding the editable "About me" content.
/// </summary>
[Table("profile")]
public class Profile : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("full_name")]
    public string FullName { get; set; } = "";

    [Column("headline")]
    public string Headline { get; set; } = "";

    [Column("bio")]
    public string Bio { get; set; } = "";

    [Column("location")]
    public string? Location { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("github_url")]
    public string? GitHubUrl { get; set; }

    [Column("linkedin_url")]
    public string? LinkedInUrl { get; set; }

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }
}

[Table("skills")]
public class Skill : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    /// <summary>Grouping label, e.g. "Languages", "Frameworks", "Tools".</summary>
    [Column("category")]
    public string Category { get; set; } = "General";

    /// <summary>Proficiency 1-5, used to render a small meter.</summary>
    [Column("level")]
    public int Level { get; set; } = 3;

    [Column("sort_order")]
    public int SortOrder { get; set; }
}

[Table("upcoming_projects")]
public class UpcomingProject : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    /// <summary>Free-text status, e.g. "Planned", "In progress", "Researching".</summary>
    [Column("status")]
    public string Status { get; set; } = "Planned";

    [Column("target_date")]
    public string? TargetDate { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; }
}

[Table("messages")]
public class ContactMessage : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("email")]
    public string Email { get; set; } = "";

    [Column("message")]
    public string Message { get; set; } = "";

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
}
