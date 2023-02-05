using Models;
using Repository.interfaces;

namespace Repository.repositories;

public class TasksRepository : DataBaseRepository<DescriptionTask, RepositoryContext>, ITasksRepository
{
    public TasksRepository(RepositoryContext context) : base(context)
    {
    }

}