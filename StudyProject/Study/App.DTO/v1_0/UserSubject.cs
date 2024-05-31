namespace App.DTO.v1_0;

public class UserSubject
{
    public Guid Id { get; set; }
    
    public int Grade { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public Guid SubjectId { get; set; }
}