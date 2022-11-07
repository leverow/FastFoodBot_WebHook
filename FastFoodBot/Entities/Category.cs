using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodBot.Entities;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Parent {get; set;}
    public List<Category>? Child { get; set; }
    public List<Product>? Products { get; set; }
}