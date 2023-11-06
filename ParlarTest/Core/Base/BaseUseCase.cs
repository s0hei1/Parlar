using ParlarTest.Data.DB;
namespace ParlarTest.Core.Base;

public abstract class BaseUseCase
{
    protected readonly MyDBContext db;

    public BaseUseCase(MyDBContext myDbContext)
    {
        db = myDbContext;
    }

    protected abstract bool TableExists();
}