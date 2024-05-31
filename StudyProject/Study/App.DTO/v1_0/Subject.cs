namespace App.DTO.v1_0;

public class Subject
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public Guid SemesterId { get; set; }
}