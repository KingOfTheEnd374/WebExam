namespace App.DTO.v1_0;

public class Homework
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public Guid SubjectId { get; set; }
}