using System.Threading.Tasks;
using CashFlow.Command.Abstractions;
using CashFlow.Command.Repositories;
using FluentValidation;
using MediatR;

namespace CashFlow.Command.CommandHandlers
{
    internal sealed class AddTransactionCommandHandler : SafeCommandHandler<AddTransactionCommand>
    {
        private readonly ITransactionRepository _repository;

        public AddTransactionCommandHandler(ITransactionRepository repository)
        {
            _repository = repository;
        }

        protected override void DefineRules()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FinancialYearId).NotEmpty();
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.AmountInCents).NotEqual(0);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Comment).MaximumLength(250);
            RuleFor(x => x.CodeNames).NotNull();
        }

        protected override async Task<Unit> HandleValidatedCommand(AddTransactionCommand command)
        {
            await _repository.Add(
                id: command.Id,
                financialYearId: command.FinancialYearId,
                accountId: command.AccountId,
                supplierId: command.SupplierId,
                amountInCents: command.AmountInCents,
                isInternalTransfer: false,
                description: command.Description,
                comment: command.Comment,
                codeNames: command.CodeNames);
            return Unit.Value;
        }
    }

    internal sealed class AssignCodeToTransactionCommandHandler : SafeCommandHandler<AssignCodeToTransactionCommand>
    {
        private readonly ITransactionRepository _repository;

        public AssignCodeToTransactionCommandHandler(ITransactionRepository repository)
        {
            _repository = repository;
        }

        protected override void DefineRules()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CodeName).NotEmpty();
        }

        protected override async Task<Unit> HandleValidatedCommand(AssignCodeToTransactionCommand command)
        {
            await _repository.AssignCode(
                id: command.Id,
                codeName: command.CodeName);
            return Unit.Value;
        }
    }

    internal sealed class UnassignCodeFromTransactionCommandHandler : SafeCommandHandler<UnassignCodeFromTransactionCommand>
    {
        private readonly ITransactionRepository _repository;

        public UnassignCodeFromTransactionCommandHandler(ITransactionRepository repository)
        {
            _repository = repository;
        }

        protected override void DefineRules()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CodeName).NotEmpty();
        }

        protected override async Task<Unit> HandleValidatedCommand(UnassignCodeFromTransactionCommand command)
        {
            await _repository.UnassignCode(
                id: command.Id,
                codeName: command.CodeName);
            return Unit.Value;
        }
    }
}
