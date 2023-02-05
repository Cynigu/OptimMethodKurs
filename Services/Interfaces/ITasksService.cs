using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsView;

namespace Services.Interfaces
{
    public interface ITasksService
    {
        public Task<ICollection<TaskView>> GetAllTaskAsync();
        public ICollection<TaskView> GetAllTask();
        public Task<int> AddTaskAsync(string name, string desc, string? realizationName);
        public Task DeleteTaskAsync(int idTask);
        public Task EditTaskAsync(int idTask, string name, string desc, string? realizationName);
        public ICollection<TaskParameterValueView> GetAllParametersValues();
        public Task<ICollection<TaskParameterValueView>> GetAllParametersAsync();
        public Task<ICollection<TaskParameterValueView>> GetParametersByTaskIdAsync(int taskId);
        public ICollection<TaskParameterValueView> GetParametersByTaskId(int taskId);
        public Task AddParameterTaskAsync(int parameterId, int taskId, double value);
        public Task DeleteParameterTaskAsync(int parameterId, int taskId);
        public Task EditParameterTaskAsync(int parameterId, int taskId, double value);
    }
}
