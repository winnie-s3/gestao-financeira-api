namespace GestaoAPI.Models.Entities
{
    public class Category
    {
        public int CategoryId {  get; set; }
        public string Name { get; set; } = "";
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
