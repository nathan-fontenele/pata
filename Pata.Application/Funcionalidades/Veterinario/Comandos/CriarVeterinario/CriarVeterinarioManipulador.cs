using Pata.Application.Comum.Mensagens;
using Pata.Domain.ObjetosValor;
using Pata.Domain.Repositorios;
using VeterinarioAgregado = Pata.Domain.Entidades.Veterinario.Veterinario;

namespace Pata.Application.Funcionalidades.Veterinario.Comandos.CriarVeterinario;

public sealed class CriarVeterinarioManipulador(IRepositorioVeterinario repositorioVeterinario)
    : IManipuladorComando<CriarVeterinarioComando, Guid>
{
    public async Task<Guid> Handle(
        CriarVeterinarioComando comando,
        CancellationToken tokenCancelamento)
    {
        var crmv = new Crmv(comando.NumeroCrmv, comando.UfCrmv);

        if (await repositorioVeterinario.ExisteCrmvIncluindoExcluidosAsync(
                crmv,
                tokenCancelamento))
            throw new InvalidOperationException("Ja existe um veterinario com o CRMV informado.");

        var veterinario = new VeterinarioAgregado(
            comando.Nome,
            new Email(comando.Email),
            new Telefone(comando.Telefone),
            crmv,
            comando.Especialidade);

        await repositorioVeterinario.AdicionarAsync(veterinario, tokenCancelamento);

        return veterinario.Id;
    }
}
