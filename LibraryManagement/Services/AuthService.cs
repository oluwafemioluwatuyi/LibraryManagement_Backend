using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LibraryManagement.Constants;
using LibraryManagement.DTOs;
using LibraryManagement.Helpers;
using LibraryManagement.Interfaces.Other;
using LibraryManagement.Interfaces.Repositories;
using LibraryManagement.Interfaces.Services;
using LibraryManagement.Models;
using LibraryManagement.Utils;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConstants _constants;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IEmailService emailService;
        private readonly IConfiguration Configuration;

        public AuthService(IUserRepository userRepository, IMapper mapper, IConstants constants, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _constants = constants;
            _httpContextAccessor = httpContextAccessor;
            // this.emailService = emailService;
            Configuration = configuration;
        }
        public async Task<ServiceResponse<object>> RegisterAsync(RegisterRequestDto registerRequestDto)
        {

            // Checking if user doesn't already exists with that email
            var alreadyExistingUser = await _userRepository.GetByEmailAsync(registerRequestDto.Email);

            if (alreadyExistingUser != null && alreadyExistingUser.EmailConfirmed)
            {
                return new ServiceResponse<object>(ResponseStatus.BadRequest, AppStatusCodes.AlreadyExists, "User already exists", null);
            }

            User user;

            if (alreadyExistingUser is null)
            {
                // Map from dto to user model
                user = _mapper.Map<User>(registerRequestDto);

                user.Id = Guid.NewGuid();

                // Hashing the password before saving in the database
                user.Password = HashPassword(registerRequestDto.Password);
            }
            else
            {
                // Doing this so that users without verified emails are overwritten
                user = alreadyExistingUser;
            }
            // Set email verification data
            var emailVerificationToken = RandomCharacterGenerator.GenerateRandomString(_constants.EMAIL_VERIFICATION_TOKEN_LENGTH);
            user.EmailVerificationToken = emailVerificationToken;
            user.EmailVerificationTokenExpiration = DateTime.UtcNow.AddMinutes(_constants.EMAIL_VERIFICATION_TOKEN_EXPIRATION_MINUTES);

            if (alreadyExistingUser is null)
            {
                // Saving user
                _userRepository.Add(user);
            }
            else
            {
                // Modifying user
                _userRepository.MarkAsModified(user);
            }

            // Committing user changes
            var userSaveResult = await _userRepository.SaveChangesAsync();
            if (!userSaveResult)
            {
                throw new Exception();
            }

            // Sending verification mail to newly registered user
            // await SendVerificationMail(user, emailVerificationToken);
            return new ServiceResponse<object>(ResponseStatus.Success, AppStatusCodes.Success, "Registered successfully", new
            {
                user = _mapper.Map<UserDto>(user)
            });



        }





        public async Task<ServiceResponse<object>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return new ServiceResponse<object>(ResponseStatus.BadRequest, AppStatusCodes.InvalidCredentials, "Invalid credentials", null);
            }

            // Verifying if password provided matches the saved hashed password
            if (!VerifyPassword(loginDto.Password, user.Password))
            {
                return new ServiceResponse<object>(ResponseStatus.BadRequest, AppStatusCodes.InvalidCredentials, "Invalid credentials", null);
            }
            ;

            if (!user.EmailConfirmed)
            {
                return new ServiceResponse<object>(ResponseStatus.BadRequest, AppStatusCodes.EmailNotVerified, "Email yet to be verified", null);
            }

            // Creating JWT
            string token = CreateJwtToken(user);

            var userDto = _mapper.Map<UserDto>(user);

            return new ServiceResponse<object>(ResponseStatus.Success, AppStatusCodes.Success, "Login Successful", new
            {
                message = "Login successfull",
                token,
                user = userDto
            });
        }






        private string CreateJwtToken(User user)
        {

            // Declaring claims we would like to write to the JWT
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // new Claim(ClaimTypes.Role, user.Role.ToString())

            };

            // Creating a new SymmetricKey from Token we have saved in appSettings.development.json file
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:Token").Value));

            // Declaring signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Creating new JWT object
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            // Write JWT to a string
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string HashPassword(string password)
        {

            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private bool VerifyPassword(string password, string passwordHash)
        {

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        //     private async Task SendVerificationMail(User user, string emailVerificationToken)
        //     {
        //         try
        //         {
        //             await emailService.SendHtmlEmailAsync(user.Email, "Verify your email", "VerifyEmail", new
        //             {
        //                 Name = user.FirstName,
        //                 EmailVerificationToken = emailVerificationToken,
        //                 UserId = user.Id,
        //                 Email = user.Email
        //             });
        //         }
        //         catch (Exception e)
        //         {
        //             Console.WriteLine(e);
        //             Console.WriteLine("Failed to send verification email");
        //         }
        //     }
    }
}


