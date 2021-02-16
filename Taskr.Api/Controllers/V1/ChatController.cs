﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskr.Commands.Chat;
using Taskr.Dtos.Chat;
using Taskr.Queries.Chat;

namespace Taskr.Api.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create/{jobId}/{taskrId}")]
        public async Task<ActionResult<ChatCreateReturnDto>> CreateChat(Guid jobId, string taskrId, CancellationToken ct)
        {
            var createChatCommand = new CreateChatCommand(taskrId, jobId);
            return await _mediator.Send(createChatCommand, ct);
        }

        [HttpGet]
        public async Task<ActionResult<List<ChatDto>>> GetAllChats([FromQuery] string predicate, CancellationToken ct)
        {
            var query = new GetAllChats(predicate);
            return await _mediator.Send(query, ct);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<SingleChatDto>> GetChatById(Guid chatId, CancellationToken ct)
        {
            var query = new GetChatById(chatId);
            return await _mediator.Send(query, ct);
        }
    }
}