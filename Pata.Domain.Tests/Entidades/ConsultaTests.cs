using Pata.Domain.Entidades.Consulta;
using Pata.Domain.Entidades.Prontuario;
using Pata.Domain.Excecoes;

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

        Assert.Throws<ErroDeValidacao>(() =>
            new Consulta(animalId, veterinarioId, tutorId, DataHora, DataAtual));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void DeveRejeitarAgendamentoQueNaoEstejaNoFuturo(int minutos)
    {
        var dataHora = DataAtual.AddMinutes(minutos);

        Assert.Throws<ErroDeValidacao>(() =>
            new Consulta(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), dataHora, DataAtual));
    }

    [Fact]
    public void DeveConfirmarConsultaAgendada()
    {
        var consulta = CriarConsulta();

        consulta.Confirmar(DataAtual);

        Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
        Assert.Equal(DataAtual, consulta.ConfirmadaEm);
    }

    [Fact]
    public void DeveRejeitarConfirmacaoSemData()
    {
        var consulta = CriarConsulta();

        Assert.Throws<ErroDeValidacao>(() => consulta.Confirmar(default));
        Assert.Equal(StatusConsulta.Agendada, consulta.Status);
        Assert.Null(consulta.ConfirmadaEm);
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
        consulta.Confirmar(DataAtual);

        consulta.Cancelar();

        Assert.Equal(StatusConsulta.Cancelada, consulta.Status);
    }

    [Fact]
    public void DeveRealizarConsultaConfirmada()
    {
        var consulta = CriarConsulta();
        consulta.Confirmar(DataAtual);

        RealizarConsulta(consulta);

        Assert.Equal(StatusConsulta.Finalizada, consulta.Status);
        Assert.NotNull(consulta.Prontuario);
        Assert.Equal(consulta.Id, consulta.Prontuario.ConsultaId);
    }

    [Fact]
    public void DeveRemarcarConsultaConfirmadaComoAgendada()
    {
        var consulta = CriarConsulta();
        var novaDataHora = DataHora.AddDays(1);
        consulta.Confirmar(DataAtual);

        consulta.Remarcar(novaDataHora, DataAtual);

        Assert.Equal(novaDataHora, consulta.DataHora);
        Assert.Equal(StatusConsulta.Agendada, consulta.Status);
        Assert.Null(consulta.ConfirmadaEm);
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

        Assert.Throws<ErroDeValidacao>(() => consulta.SubstituirVeterinario(Guid.Empty));
        Assert.Equal(veterinarioOriginal, consulta.VeterinarioId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void DeveRejeitarRemarcacaoQueNaoEstejaNoFuturoSemAlterarConsulta(int minutos)
    {
        var consulta = CriarConsulta();

        Assert.Throws<ErroDeValidacao>(() =>
            consulta.Remarcar(DataAtual.AddMinutes(minutos), DataAtual));
        Assert.Equal(DataHora, consulta.DataHora);
        Assert.Equal(StatusConsulta.Agendada, consulta.Status);
    }

    [Fact]
    public void DeveFinalizarConsultaAgendada()
    {
        var consulta = CriarConsulta();

        RealizarConsulta(consulta);

        Assert.Equal(StatusConsulta.Finalizada, consulta.Status);
    }

    [Fact]
    public void DeveRejeitarConfirmacaoDeConsultaJaConfirmada()
    {
        var consulta = CriarConsulta();
        consulta.Confirmar(DataAtual);

        Assert.Throws<RegraDeNegocioException>(() => consulta.Confirmar(DataAtual));
        Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
    }

    [Theory]
    [InlineData(StatusConsulta.Cancelada)]
    [InlineData(StatusConsulta.Finalizada)]
    public void DeveImpedirAlteracoesEmConsultaEncerrada(StatusConsulta statusFinal)
    {
        var consulta = CriarConsultaEncerrada(statusFinal);
        var dataHoraOriginal = consulta.DataHora;
        var veterinarioOriginal = consulta.VeterinarioId;

        Assert.Throws<RegraDeNegocioException>(() =>
            consulta.Remarcar(DataHora.AddDays(1), DataAtual));
        Assert.Throws<RegraDeNegocioException>(() =>
            consulta.SubstituirVeterinario(Guid.NewGuid()));
        Assert.Throws<RegraDeNegocioException>(consulta.Cancelar);
        Assert.Throws<RegraDeNegocioException>(() => consulta.Confirmar(DataAtual));
        Assert.Throws<RegraDeNegocioException>(() => RealizarConsulta(consulta));

        Assert.Equal(dataHoraOriginal, consulta.DataHora);
        Assert.Equal(veterinarioOriginal, consulta.VeterinarioId);
        Assert.Equal(statusFinal, consulta.Status);
    }

    private static Consulta CriarConsulta() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DataHora, DataAtual);

    private static Consulta CriarConsultaEncerrada(StatusConsulta statusFinal)
    {
        var consulta = CriarConsulta();

        if (statusFinal == StatusConsulta.Finalizada)
        {
            consulta.Confirmar(DataAtual);
            RealizarConsulta(consulta);
        }
        else
        {
            consulta.Cancelar();
        }

        return consulta;
    }

    private static void RealizarConsulta(Consulta consulta) =>
        consulta.Finalizar(
            [new Sintoma("Febre")],
            "Virose",
            "Repouso e hidratacao",
            DataHora);
}
