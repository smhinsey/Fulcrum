using Fulcrum.Core;

namespace Fulcrum.Tests.ExampleUserSystem.Events
{
  public class UserRegistered : IEvent
  {
    public string DisplayName { get; set; }

    public string EmailAddress { get; set; }

    public string PasswordHash { get; set; }
  }
}