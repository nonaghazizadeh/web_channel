﻿using OOD_Project_Backend.Channel.ChannelCore.DataAccess.Entities;
using OOD_Project_Backend.Core.DataAccess.Contracts;

namespace OOD_Project_Backend.Channel.ChannelCore.DataAccess.Repositories.Contracts;

public interface IChannelPremiumUsersRepository : IBaseRepository<ChannelPremiumUsersEntity>
{
    ValueTask<ChannelPremiumUsersEntity?> Find(int userId, int channelId);
    Task<List<ChannelPremiumUsersEntity>> FindAfterDate(DateTime dateTime);
}