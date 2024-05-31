using Base.DAL.EF;

namespace Base.Test.DAL;

public class TestUOW : BaseUnitOfWork<TestDbContext>
{
    public TestUOW(TestDbContext dbContext) : base(dbContext)
    {
    }
}