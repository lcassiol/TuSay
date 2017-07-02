using SQLite;

namespace Todo
{
	[Table("todoitem")]
	public class TodoItem
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string beforeWords { get; set; }
		public string afterWords { get; set; }
	}
}
