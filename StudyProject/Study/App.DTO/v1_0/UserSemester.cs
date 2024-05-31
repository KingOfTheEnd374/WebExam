namespace App.DTO.v1_0;

public class UserSemester
{
    public Guid Id { get; set; }
    
    public int Grade { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public Guid SemesterId { get; set; }
}