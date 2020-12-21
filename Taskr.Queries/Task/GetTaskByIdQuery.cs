﻿using System;
using MediatR;
using Taskr.Domain;

namespace Taskr.Queries
{
    public class GetTaskByIdQuery : IRequest<Task>
    {
        public Guid TaskId { get; set; }

        public GetTaskByIdQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}