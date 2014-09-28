namespace Fulcrum.Tests.ExampleUserSystem.QueryModel
{
  public class User
  {
    public string EmailAddress { get; set; }

    public string PasswordHash { get; set; }

    public UserState State { get; set; }

    public Profile PublicProfile { get; set; }
  }
}
