using System.ComponentModel.DataAnnotations;

namespace User.API.Models
{
	public class BaseEntity
	{
		public DateTime? DateCreated { get; set; }
		public DateTime? LastModifiedDate { get; set; }
		public string? LastModifiedBy { get; set; }
		public string? Status { get; set; }
	}
}
