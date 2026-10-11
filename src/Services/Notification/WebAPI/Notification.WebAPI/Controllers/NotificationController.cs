using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Features.Notifications.Queries;

namespace Notification.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
	private readonly ISender _sender;

	public NotificationController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAll(CancellationToken cancellationToken)
	{
		var notifications = await _sender.Send(new GetNotificationsQuery(), cancellationToken);
		return Ok(notifications);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<NotificationDto>> GetById(long id, CancellationToken cancellationToken)
	{
		var notification = await _sender.Send(new GetNotificationByIdQuery { Id = id }, cancellationToken);
		if (notification is null)
			return NotFound();

		return Ok(notification);
	}
}
