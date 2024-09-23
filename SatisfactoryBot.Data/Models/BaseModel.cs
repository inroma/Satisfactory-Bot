namespace SatisfactoryBot.Data.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;


[PrimaryKey(nameof(Id))]
public abstract class BaseModel
{
    public int Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreationTime { get; set; } = DateTime.UtcNow;
}
