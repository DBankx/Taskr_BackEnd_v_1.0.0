using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Taskr.Commands.Task;
using Taskr.Persistance;
using Taskr.RepositoryServices.TaskService;

namespace Taskr.Handlers.Task
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskService _taskService;
        private readonly DataContext _context;

        public UpdateTaskHandler(ITaskService taskService, DataContext context)
        {
            _taskService = taskService;
            _context = context;
        }
        
        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetTaskByIdAsync(request.Id);
            if (task == null)
            {
                return false;
            }

            task.Description = request.Description ?? task.Description;
            task.Title = request.Title ?? task.Title;
            task.InitialPrice = request.InitialPrice ?? task.InitialPrice;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}