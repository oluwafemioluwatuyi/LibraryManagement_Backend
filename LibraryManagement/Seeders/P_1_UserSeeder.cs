using LibraryManagement.Interfaces.Other;
using LibraryManagement.Interfaces.Repositories;
using LibraryManagement.Models;

namespace LibraryManagement.Seeders
{
    public class P_1_UserSeeder : ISeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IConstants _constants;

        public P_1_UserSeeder(IUserRepository userRepository, IConstants constants)
        {
            _userRepository = userRepository;
            _constants = constants;
        }

        public Task down()
        {
            throw new NotImplementedException();
        }

        public async Task up()
        {
            Console.WriteLine("Seeding user...");
            var systemUser = new User
            {
                FirstName = _constants.SYSTEM_USER_FIRST_NAME,
                LastName = _constants.SYSTEM_USER_LAST_NAME,
                Email = _constants.SYSTEM_USER_EMAIL,

            };

            _userRepository.Add(systemUser);

            await _userRepository.SaveChangesAsync();
        }

        public string Description()
        {
            throw new NotImplementedException();
        }


    }
}
