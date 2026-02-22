using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Models;

[Table("watched")]
public class WatchedMovie
{
    [Key]
    [Column("letterboxd_uri")]
    public string LetterboxdUri { get; set; } = string.Empty;

    [Required]
    [Column("watch_date")]
    public DateOnly WatchDate { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column("release_year")]
    [Range(1878, 2100)]
    public short ReleaseYear { get; set; }

    [Column("cache_id")]
    public Guid? CacheId { get; set; }

    [Column("genres")]
    public string Genres { get; set; }
}