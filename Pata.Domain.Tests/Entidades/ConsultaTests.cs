using Pata.Domain.Entidades.Consulta;
using Pata.Domain.Entidades.Prontuario;

namespace Pata.Domain.Tests.Entidades;

public class ConsultaTests
{
    private static readonly DateTimeOffset DataAtual = new(2026, 9, 8, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset DataHora = DataAtual.AddDays(1);

    [Fact]
    public void DeveAgendarConsultaComDadosValidos()
    {
        var animalId = Guid.NewGuid();
        var veterinarioId = Guid.NewGuid();
        var tutorId = Guid.NewGuid();

        var consulta = new Consulta(animalId, veterinarioId, tutorId, DataHora, DataAtual);

        Assert.NotEqual(Guid.Empty, consulta.Id);
        Assert.Equal(animalId, consulta.AnimalId);
        Assert.Equal(veterinarioId, consulta.VeterinarioId);
        Assert.Equal(tutorId, consulta.TutorId);
        Assert.Equal(DataHora, consulta.DataHora);
        Assert.Equal(StatusConsulta.Agendada, consulta.Status);
    }

    [Theory]
    [InlineData("animal")]
    [InlineData("veterinario")]
    [InlineData("tutor")]
    public void DeveRejeitarIdentificadorObrigatorioVazio(string identificador)
    {
        var animalId = identificador == "animal" ? Guid.Empty : Guid.NewGuid();
        var veterinarioId = identificador == "veterinario" ? Guid.Empty : Guid.NewGuid();
        var tutorId = identificador == "tutor" ? Guid.Empty : Guid.NewGuid();

        Assert.Throws<ArgumentException>(() =>
            new Consulta(animalId, veterinarioId, tutorId, DataHora, DataAtual));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void DeveRejeitarAgendamentoQueNaoEstejaNoFuturo(int minutos)
    {
        var dataHora = DataAtual.AddMinutes(minutos);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Consulta(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), dataHora, DataAtual));
    }

    [Fact]
    public void DeveConfirmarConsultaAgendada()
    {
        var consulta = CriarConsulta();

        consulta.Confirmar();

        Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
    }

    [Fact]
    public void DeveCancelarConsultaAgendada()
    {
        var consulta = CriarConsulta();

        consulta.Cancelar();

        Assert.Equal(StatusConsulta.Cancelada, consulta.Status);
    }

    [Fact]
    public void DeveCancelarConsultaConfirmada()
    {
        var consulta = CriarConsulta();
        consulta.Confirmar();

        consulta.Cancelar();

        Assert.Equal(StatusConsulta.Cancelada, consulta.Status);
    }

    [Fact]
    public void DeveRealizarConsultaConfirmada()
    {
        var consulta = CriarConsulta();
        consulta.Confirmar();

        RealizarConsulta(consulta);

        Assert.Equal(StatusConsulta.Realizada, consulta.Status);
        Assert.NotNull(consulta.Prontuario);
        Assert.Equal(consulta.Id, consulta.Prontuario.ConsultaId);
    }

    [Fact]
    public void DeveRemarcarConsultaConfirmadaComoAgendada()
    {
        var consulta = CriarConsulta();
        var novaDataHora = DataHora.AddDays(1);
        consulta.Confirmar();

        consulta.Remarcar(novaDataHora, DataAtual);

        Assert.Equal(novaDataHora, consulta.DataHora);
        Assert.Equal(StatusConsulta.Agendada, consulta.Status);
    }

    [Fact]
    public void DeveSubstituirVeterinarioEmConsultaAberta()
    {
        var consulta = CriarConsulta();
        var novoVeterinarioId = Guid.NewGuid();

        consulta.SubstituirVeterinario(novoVeterinarioId);

        Assert.Equal(novoVeterinarioId, consulta.VeterinarioId);
    }

    [Fact]
    public void DeveRejeitarVeterinarioVazioSemAlterarConsulta()
    {
        var consulta = CriarConsulta();
        var veterinarioOriginal = consulta.VeterinarioId;

        Assert.Throws<ArgumentException>(() => consulta.SubstituirVeterinario(Guid.Empty));
        Assert.Equal(veterinarioOriginal, consulta.VeterinarioId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void DeveRejeitarRemarcacaoQueNaoEstejaNoFuturoSemAlterarConsulta(int minutos)
    {
        var consulta = CriarConsulta();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            consulta.Remarcar(DataAtual.AddMinutes(minutos), DataAtual));
        Assert.Equal(DataHora, consulta.DataHora);
        Assert.Equal(StatusConsulta.Agendada, consulta.Status);
    }

    [Fact]
    public void DeveRejeitarRealizacaoDeConsultaNaoConfirmada()
    {
        var consulta = CriarConsulta();

        Assert.Throws<InvalidOperationException>(() => RealizarConsulta(consulta));
        Assert.Equal(StatusConsulta.Agendada, consulta.Status);
    }

    [Fact]
    public void DeveRejeitarConfirmacaoDeConsultaJaConfirmada()
    {
        var consulta = CriarConsulta();
        consulta.Confirmar();

        Assert.Throws<InvalidOperationException>(consulta.Confirmar);
        Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
    }

    [Theory]
    [InlineData(StatusConsulta.Cancelada)]
    [InlineData(StatusConsulta.Realizada)]
    public void DeveImpedirAlteracoesEmConsultaEncerrada(StatusConsulta statusFinal)
    {
        var consulta = CriarConsultaEncerrada(statusFinal);
        var dataHoraOriginal = consulta.DataHora;
        var veterinarioOriginal = consulta.VeterinarioId;

        Assert.Throws<InvalidOperationException>(() =>
            consulta.Remarcar(DataHora.AddDays(1), DataAtual));
        Assert.Throws<InvalidOperationException>(() =>
            consulta.SubstituirVeterinario(Guid.NewGuid()));
        Assert.Throws<InvalidOperationException>(consulta.Cancelar);
        Assert.Throws<InvalidOperationException>(consulta.Confirmar);
        Assert.Throws<InvalidOperationException>(() => RealizarConsulta(consulta));

        Assert.Equal(dataHoraOriginal, consulta.DataHora);
        Assert.Equal(veterinarioOriginal, consulta.VeterinarioId);
        Assert.Equal(statusFinal, consulta.Status);
    }

    private static Consulta CriarConsulta() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DataHora, DataAtual);

    private static Consulta CriarConsultaEncerrada(StatusConsulta statusFinal)
    {
        var consulta = CriarConsulta();

        if (statusFinal == StatusConsulta.Realizada)
        {
            consulta.Confirmar();
            RealizarConsulta(consulta);
        }
        else
        {
            consulta.Cancelar();
        }

        return consulta;
    }

    private static void RealizarConsulta(Consulta consulta) =>
        consulta.Realizar(
            [new Sintoma("Febre")],
            "Virose",
            "Repouso e hidratacao",
            DataHora);
}
