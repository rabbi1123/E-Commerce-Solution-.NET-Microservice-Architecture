namespace Eventbus.Messages.Events
{
	public class UserRegistrationEvent : IntegrationBaseEvent
	{
		public Guid UserId { get; set; }
		public string Email { get; set; }
	}
}
