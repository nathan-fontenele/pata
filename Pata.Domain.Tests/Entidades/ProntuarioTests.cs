using Pata.Domain.Entidades.Consulta;
using Pata.Domain.Entidades.Prontuario;

namespace Pata.Domain.Tests.Entidades;

public class ProntuarioTests
{
    private static readonly DateTimeOffset DataAtual = new(2026, 9, 8, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset DataHora = DataAtual.AddDays(1);

    [Fact]
    public void DeveRegistrarProntuarioComUmSintoma()
    {
        var consulta = CriarConsultaConfirmada();
        var sintoma = new Sintoma("  Febre  ");

        consulta.Realizar([sintoma], "  Virose  ", "  Repouso  ", DataHora);

        var prontuario = Assert.IsType<Prontuario>(consulta.Prontuario);
        Assert.NotEqual(Guid.Empty, prontuario.Id);
        Assert.Equal(consulta.Id, prontuario.ConsultaId);
        Assert.Equal("Febre", Assert.Single(prontuario.Sintomas).Descricao);
        Assert.Equal("Virose", prontuario.Diagnostico);
        Assert.Equal("Repouso", prontuario.Prescricao);
        Assert.Equal(DataHora, prontuario.DataRegistro);
        Assert.Equal(StatusConsulta.Realizada, consulta.Status);
    }

    [Fact]
    public void DeveRegistrarProntuarioComVariosSintomas()
    {
        var consulta = CriarConsultaConfirmada();

        consulta.Realizar(
            [new Sintoma("Febre"), new Sintoma("Tosse"), new Sintoma("Apatia")],
            null,
            null,
            DataHora);

        Assert.Equal(3, consulta.Prontuario!.Sintomas.Count);
    }

    [Fact]
    public void DevePermitirDiagnosticoEPrescricaoAusentes()
    {
        var consulta = CriarConsultaConfirmada();

        consulta.Realizar([new Sintoma("Apatia")], "   ", null, DataHora);

        Assert.Null(consulta.Prontuario!.Diagnostico);
        Assert.Null(consulta.Prontuario.Prescricao);
    }

    [Fact]
    public void NaoDevePermitirAlterarColecaoDeSintomasDiretamente()
    {
        var consulta = CriarConsultaConfirmada();
        consulta.Realizar([new Sintoma("Febre")], null, null, DataHora);
        var sintomas = Assert.IsAssignableFrom<ICollection<Sintoma>>(consulta.Prontuario!.Sintomas);

        Assert.True(sintomas.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => sintomas.Add(new Sintoma("Tosse")));
    }

    [Fact]
    public void DeveRejeitarProntuarioSemSintomasSemRealizarConsulta()
    {
        var consulta = CriarConsultaConfirmada();

        Assert.Throws<ArgumentException>(() =>
            consulta.Realizar([], null, null, DataHora));
        Assert.Null(consulta.Prontuario);
        Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
    }

    [Fact]
    public void DeveRejeitarListaDeSintomasNulaSemRealizarConsulta()
    {
        var consulta = CriarConsultaConfirmada();

        Assert.Throws<ArgumentNullException>(() =>
            consulta.Realizar(null!, null, null, DataHora));
        Assert.Null(consulta.Prontuario);
        Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
    }

    [Fact]
    public void DeveRejeitarSintomasDuplicadosIgnorandoMaiusculas()
    {
        var consulta = CriarConsultaConfirmada();

        Assert.Throws<ArgumentException>(() => consulta.Realizar(
            [new Sintoma("Febre"), new Sintoma("febre")],
            null,
            null,
            DataHora));
        Assert.Null(consulta.Prontuario);
        Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
    }

    [Fact]
    public void DeveImpedirSegundoProntuarioNaMesmaConsulta()
    {
        var consulta = CriarConsultaConfirmada();
        consulta.Realizar([new Sintoma("Febre")], null, null, DataHora);
        var prontuarioOriginal = consulta.Prontuario;

        Assert.Throws<InvalidOperationException>(() => consulta.Realizar(
            [new Sintoma("Tosse")],
            null,
            null,
            DataHora));
        Assert.Same(prontuarioOriginal, consulta.Prontuario);
    }

    [Fact]
    public void DeveRejeitarDataDeRegistroVazia()
    {
        var consulta = CriarConsultaConfirmada();

        Assert.Throws<ArgumentException>(() =>
            consulta.Realizar([new Sintoma("Febre")], null, null, default));
        Assert.Null(consulta.Prontuario);
        Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void DeveRejeitarDescricaoDeSintomaVazia(string descricao)
    {
        Assert.Throws<ArgumentException>(() => new Sintoma(descricao));
    }

    [Fact]
    public void DeveRejeitarDescricaoDeSintomaAcimaDoLimite()
    {
        var descricao = new string('A', Sintoma.TamanhoMaximoDescricao + 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => new Sintoma(descricao));
    }

    private static Consulta CriarConsultaConfirmada()
    {
        var consulta = new Consulta(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DataHora,
            DataAtual);
        consulta.Confirmar();
        return consulta;
    }
}
