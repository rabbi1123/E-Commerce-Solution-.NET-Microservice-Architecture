using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Common
{
	public class BaseDomainEntity
	{
		[Key]
		public int Id { get; set; }
		public DateTime? DateCreated { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? LastModifiedDate { get; set; }
		public string? LastModifiedBy { get; set; }
		public string? Status { get; set; }
	}
}
