using DomainService.Services;
using DomainService.Specifications;
using MediatR;
using onlineshop.Data;
using onlineshop.Exceptions;
using Resources;

namespace Application.Commands.BackOfficeUser.Login;

public class LoginCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService) : IRequestHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var specification = new GetBackOfficeUserWithUsernameAndPasswordSpecification(request.Username, request.Password);
        var backOfficeUser = await unitOfWork.BackOfficeUserRepository.GetAsync(specification, cancellationToken)
            ?? throw new NotFoundException(Messages.LoginFailed);

        backOfficeUser.SetLastLoginAt();

        unitOfWork.BackOfficeUserRepository.Update(backOfficeUser);
        await unitOfWork.CommitAsync(cancellationToken);

        var permissions = backOfficeUser.Roles.SelectMany(x => x.Permissions).Select(x => x.Type.ToString()).ToList();

        var token = tokenService.Generate(backOfficeUser.Username, permissions);

        return token;
    }
}
