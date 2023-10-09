﻿using Microsoft.AspNetCore.Mvc;

namespace OOD_Project_Backend.Core.Context;

public class Response : ObjectResult
{
    public object? Message { get; init; }
    public Response(int statusCode,object? message) : base(message)
    {
        base.StatusCode = statusCode;
        Message = message;
    }
}