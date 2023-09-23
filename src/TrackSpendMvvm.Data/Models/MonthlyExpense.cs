using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackSpendMvvm.Data.Models;

public class MonthlyExpense
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public string Id { get; set; } = Guid.NewGuid().ToString();

	[Required, StringLength(256)]
	public string Title { get; set; } = string.Empty;

	[StringLength(1_024)]
	public string? Description { get; set; }

	public decimal Amount { get; set; }

	[Range(1, 31)]
	public int DayOfMonth { get; set; }
}