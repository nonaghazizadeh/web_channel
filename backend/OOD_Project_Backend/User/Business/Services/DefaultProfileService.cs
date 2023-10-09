﻿using System.Text;
using OOD_Project_Backend.Core.Context;
using OOD_Project_Backend.Core.Validation.Common;
using OOD_Project_Backend.Core.Validation.Contracts;
using OOD_Project_Backend.User.Business.Context;
using OOD_Project_Backend.User.Business.Contracts;
using OOD_Project_Backend.User.Business.Requests;
using OOD_Project_Backend.User.Business.Validations.Conditions;
using OOD_Project_Backend.User.DataAccess.Repositories.Contract;
using Exception = System.Exception;

namespace OOD_Project_Backend.User.Business.Services;

public class DefaultProfileService : IProfileService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator _validator;
    private readonly IConfiguration _configuration;
    private readonly ITokenRepository _tokenRepository;
    private readonly IEmailService _emailService;

    public DefaultProfileService(IUserRepository userRepository, IValidator validator, IConfiguration configuration,
        ITokenRepository tokenRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _validator = validator;
        _configuration = configuration;
        _tokenRepository = tokenRepository;
        _emailService = emailService;
    }

    public async Task<Response> Add(ProfileRequest profileRequest, int userId)
    {
        try
        {
            var rules = new List<IRule>()
            {
                new NameRule(profileRequest.Name),
                new BiographyRule(profileRequest.Biography),
                new EmailRule(profileRequest.Email),
                new PhoneNumberRule(profileRequest.PhoneNumber),
                new CardNumberRules(profileRequest.CardNumber)
            };
            if (!_validator.Validate(rules.ToArray()))
            {
                return new Response(400, new
                {
                    Message =
                        $"name,email,phone number,national code are required fields\nname must contains Letter or digit and biography must be at most 70 characters"
                });
            }

            var userEntity = await _userRepository.FindByUserId(userId, true);
            userEntity.Biography = profileRequest.Biography;
            userEntity.Email = profileRequest.Email;
            userEntity.PhoneNumber = profileRequest.PhoneNumber;
            userEntity.Name = profileRequest.Name;
            userEntity.CardNumber = profileRequest.CardNumber;
            _userRepository.Update(userEntity);
            await _userRepository.SaveChangesAsync();
            return new Response(200, new { Message = "Profile is Updated!" });
        }
        catch (Exception e)
        {
            return new Response(400, new { Message = "profile add request failed!" });
        }
    }

    public async Task<Response> AddProfilePicture(IFormFile picture, int userId)
    {
        try
        {
            if (!_validator.Validate(new PictureRule(picture)))
            {
                return new Response(400,
                    new { Message = "image file size must be at most 5 mb and .png and .jpg extension are supported" });
            }

            var resourcePath = _configuration.GetValue<string>("ProfilePath");
            var picPath = new StringBuilder(resourcePath).Append($"/{userId}.png").ToString();
            if (File.Exists(picPath))
            {
                File.Delete(picPath);
            }

            await using var fileStream = new FileStream(picPath, FileMode.Create);
            await picture.CopyToAsync(fileStream);
            return new Response(200, new { Message = "image uploaded successfully!" });
        }
        catch (Exception e)
        {
            return new Response(400, new { Message = "profile image upload failed!" });
        }
    }

    public async Task<Response> ShowProfile(int userId)
    {
        try
        {
            var userEntity = await _userRepository.FindByUserId(userId, false);
            if (userEntity == null)
            {
                return new Response(404, new { Message = "User not found!" });
            }

            var userDto = new UserProfile()
            {
                Biography = userEntity.Biography,
                Email = userEntity.Email,
                PhoneNumber = userEntity.PhoneNumber,
                Name = userEntity.Name,
                CardNumber = userEntity.CardNumber,
                Id = userEntity.Id
            };
            return new Response(200, new
            {
                Message = userDto
            });
        }
        catch (Exception e)
        {
            return new Response(404, new { Message = "User not found!" });
        }
    }

    public async Task<Response> RequestDeleteProfile(int userId)
    {
        try
        {
            var user = await _userRepository.FindByUserId(userId, false);
            if (user.Email == null)
            {
                return new Response(400, new { Message = "you do not have email address! update your profile!" });
            }

            var random = new Random();
            var code = random.Next(100000, 999999);
            await _tokenRepository.SaveVerificationCode(userId, code);
            await _emailService.SendEmailToUser(user.Email!, code);
            return new Response(200, new { Message = "check your email verification code has been sent" });
        }
        catch (Exception e)
        {
            return new Response(400, new { Message = "delete request failed!" });
        }
    }

    public async Task<Response> VerifyDeleteProfile(int userId, int code, string tokenId)
    {
        try
        {
            var realCode = await _tokenRepository.GetVerificationCode(userId);
            if (realCode != null)
            {
                if (realCode == code)
                {
                    var user = await _userRepository.FindByUserId(userId, false);
                    user.IsDeleted = true;
                    await _tokenRepository.SaveBlackListedTokenId(tokenId);
                    _userRepository.Update(user);
                    await _userRepository.SaveChangesAsync();
                    return new Response(200,new {Messaage = "the account is deleted!"});
                }
            }

            return new Response(400,new {Message = "failed to delete account!"});
        }
        catch (Exception e)
        {
            return new Response(400, new { Message = "the delete request failed!" });
        }
    }
}