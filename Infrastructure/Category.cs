
public class Category
{
    public int Id { get; set; }

    public DateTime Created { get; set; }
    public string CreatedBy { get; set; }
    public DateTime LastUpdated { get; set; }
    public string LastUpdatedBy { get; set; }
    public Guid UniqueId { get; set; }
    public string CategoryName { get; set; }
    public int? ParentId { get; set; }

    public List<Category> Children { get; set; } = new();
}

