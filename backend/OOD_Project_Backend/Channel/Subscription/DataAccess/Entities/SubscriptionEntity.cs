﻿using OOD_Project_Backend.Channel.ChannelCore.DataAccess.Entities;
using OOD_Project_Backend.Channel.Subscription.DataAccess.Entities.Enums;

namespace OOD_Project_Backend.Channel.Subscription.DataAccess.Entities;

public class SubscriptionEntity
{
    public int Id { get; set; }
    public int ChannelId { get; set; }
    public ChannelEntity ChannelEntity { get; set; }
    public SubscriptionPeriod Period { get; set; }
    public int Price { get; set; }
}