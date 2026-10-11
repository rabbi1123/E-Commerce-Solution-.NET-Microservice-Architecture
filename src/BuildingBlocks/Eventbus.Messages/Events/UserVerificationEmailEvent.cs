namespace Eventbus.Messages.Events
{
	public class UserVerificationEmailEvent : IntegrationBaseEvent
	{
		public Guid UserId { get; set; }

		public string Email { get; set; } = string.Empty;

		public string VerificationToken { get; set; } = string.Empty;
	}
}
