﻿namespace SatisfactoryBot.Application.Domain.NewGame;

using MediatR;

public class NewGameCommand : IRequest<bool>
{
    public ulong EntityId { get; set; }

    public string SessionName { get; set; }

    public string StartLocation { get; set; }

    public bool SkipOnboarding { get; set; }
}
