namespace LibraryManagement.Interfaces.Other
{
    public interface ISeeder
    {
        Task up();
        Task down();

        string Description();

    }
}
