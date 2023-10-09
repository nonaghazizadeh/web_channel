﻿using OOD_Project_Backend.Channel.ChannelCore.Business.Context;
using OOD_Project_Backend.Core.Context;

namespace OOD_Project_Backend.Channel.ChannelCore.Business.Contracts;

public interface IChannelService
{
    Task<Response> CreateChannel(string name);
    Task<Response> ShowChannelsList();
    Task<Response> AddChannelPicture(IFormFile formFile, int channelId);
    Task<Response> UpdateChannel(ChannelUpdateRequest updateRequest);
}