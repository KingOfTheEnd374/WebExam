namespace App.DTO.v1_0;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
}