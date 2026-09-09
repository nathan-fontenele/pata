using Pata.Domain.Comum;
using Pata.Domain.Entidades.Prontuario;
using ProntuarioClinico = Pata.Domain.Entidades.Prontuario.Prontuario;

namespace Pata.Domain.Entidades.Consulta;

public sealed class Consulta : RaizAgregadaAuditavel<Guid>
{
    private Consulta()
    {
    }

    public Consulta(
        Guid animalId,
        Guid veterinarioId,
        Guid tutorId,
        DateTimeOffset dataHora,
        DateTimeOffset dataAtual) : base(Guid.NewGuid())
    {
        ValidarIdentificador(animalId, nameof(animalId), "Animal");
        ValidarIdentificador(veterinarioId, nameof(veterinarioId), "Veterinario");
        ValidarIdentificador(tutorId, nameof(tutorId), "Tutor");
        ValidarDataFutura(dataHora, dataAtual, nameof(dataHora));

        AnimalId = animalId;
        VeterinarioId = veterinarioId;
        TutorId = tutorId;
        DataHora = dataHora;
        Status = StatusConsulta.Agendada;
    }

    public Guid AnimalId { get; private set; }
    public Guid VeterinarioId { get; private set; }
    public Guid TutorId { get; private set; }
    public DateTimeOffset DataHora { get; private set; }
    public StatusConsulta Status { get; private set; }
    public ProntuarioClinico? Prontuario { get; private set; }

    public void Confirmar()
    {
        GarantirStatus(StatusConsulta.Agendada, "confirmada");
        Status = StatusConsulta.Confirmada;
    }

    public void Cancelar()
    {
        GarantirConsultaEmAberto("cancelada");
        Status = StatusConsulta.Cancelada;
    }

    public void Realizar(
        IEnumerable<Sintoma> sintomas,
        string? diagnostico,
        string? prescricao,
        DateTimeOffset dataRegistro)
    {
        GarantirStatus(StatusConsulta.Confirmada, "realizada");

        if (Prontuario is not null)
            throw new InvalidOperationException("A consulta ja possui um prontuario.");

        var prontuario = new ProntuarioClinico(
            Guid.NewGuid(),
            Id,
            sintomas,
            diagnostico,
            prescricao,
            dataRegistro);

        Prontuario = prontuario;
        Status = StatusConsulta.Realizada;
    }

    public void Remarcar(DateTimeOffset novaDataHora, DateTimeOffset dataAtual)
    {
        GarantirConsultaEmAberto("remarcada");
        ValidarDataFutura(novaDataHora, dataAtual, nameof(novaDataHora));

        DataHora = novaDataHora;
        Status = StatusConsulta.Agendada;
    }

    public void SubstituirVeterinario(Guid veterinarioId)
    {
        GarantirConsultaEmAberto("alterada");
        ValidarIdentificador(veterinarioId, nameof(veterinarioId), "Veterinario");

        VeterinarioId = veterinarioId;
    }

    private void GarantirConsultaEmAberto(string operacao)
    {
        if (Status is StatusConsulta.Cancelada or StatusConsulta.Realizada)
            throw new InvalidOperationException(
                $"Uma consulta {Status.ToString().ToLowerInvariant()} nao pode ser {operacao}.");
    }

    private void GarantirStatus(StatusConsulta statusEsperado, string operacao)
    {
        if (Status != statusEsperado)
            throw new InvalidOperationException(
                $"A consulta deve estar {statusEsperado.ToString().ToLowerInvariant()} para ser {operacao}.");
    }

    private static void ValidarIdentificador(Guid id, string nomeParametro, string entidade)
    {
        if (id == Guid.Empty)
            throw new ArgumentException($"{entidade} e obrigatorio.", nomeParametro);
    }

    private static void ValidarDataFutura(
        DateTimeOffset dataHora,
        DateTimeOffset dataAtual,
        string nomeParametro)
    {
        if (dataHora <= dataAtual)
            throw new ArgumentOutOfRangeException(
                nomeParametro,
                "A data e hora da consulta devem ser posteriores a data atual.");
    }
}
