﻿namespace DomainService.Services;

public interface ITokenService
{
    public string Generate(string username);
}