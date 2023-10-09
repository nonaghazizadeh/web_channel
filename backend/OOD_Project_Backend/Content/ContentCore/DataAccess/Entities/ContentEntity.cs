﻿namespace OOD_Project_Backend.Content.ContentCore.DataAccess.Entities;

public class ContentEntity
{
    public int Id { get; set; }
    public string Title { get;set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}