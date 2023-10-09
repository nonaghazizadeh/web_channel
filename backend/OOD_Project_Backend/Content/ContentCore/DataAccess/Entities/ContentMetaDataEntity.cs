﻿using OOD_Project_Backend.Channel.ChannelCore.DataAccess.Entities;
using OOD_Project_Backend.Content.Category.DataAccess.Entities;
using OOD_Project_Backend.Content.ContentCore.DataAccess.Entities.Enums;

namespace OOD_Project_Backend.Content.ContentCore.DataAccess.Entities;

public class ContentMetaDataEntity
{
    public int ContentId { get; set; }
    public ContentEntity Content { get; set; }
    public int ChannelId { get; set; }
    public ChannelEntity Channel { get; set; }
    public int? CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
    public ContentType ContentType { get; set; }
    public double Price { get; set; }
    public bool Premium { get; set; }
    public string FileName { get; set; }
}