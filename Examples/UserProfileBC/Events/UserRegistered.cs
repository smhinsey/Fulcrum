using Fulcrum.Core;

namespace Examples.UserProfileBC.Events
{
  public class UserRegistered : IEvent
  {
    public string DisplayName { get; set; }

    public string EmailAddress { get; set; }

    public string PasswordHash { get; set; }
  }
}