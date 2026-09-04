using System.ComponentModel.DataAnnotations;

namespace Izotoff.Models;

public class News
{
    public const int MaxImages = 3;
    public const string TonePrefix = "tone:";

    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "Заголовок")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    [Display(Name = "Описание")]
    public string Summary { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Дата публикации")]
    public DateTime PublishedAt { get; set; }

    [StringLength(1500)]
    public string ImagePaths { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public IReadOnlyList<string> GetImageTokens()
    {
        if (string.IsNullOrWhiteSpace(ImagePaths))
            return [];

        return ImagePaths
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Take(MaxImages)
            .ToList();
    }

    public void SetImageTokens(IEnumerable<string> tokens) =>
        ImagePaths = string.Join(';', tokens.Where(token => !string.IsNullOrWhiteSpace(token)).Take(MaxImages));

    public HomeNewsItem ToHomeItem()
    {
        var tokens = GetImageTokens();
        if (tokens.Count == 0)
            tokens = ["tone:forest"];

        return new HomeNewsItem
        {
            Id = Id,
            Title = Title,
            Summary = Summary,
            PublishedAt = PublishedAt,
            Slides = tokens.Select(NewsMediaSlide.FromToken).ToList()
        };
    }
}
