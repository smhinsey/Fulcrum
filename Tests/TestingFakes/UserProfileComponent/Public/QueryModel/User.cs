namespace Examples.UserProfileComponent.Public.QueryModel
{
	public class User
	{
		public string EmailAddress { get; set; }

		public string PasswordHash { get; set; }

		public Profile PublicProfile { get; set; }

		public UserState State { get; set; }
	}
}
