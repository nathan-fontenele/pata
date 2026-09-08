using Pata.Domain.Entidades.Animal;
using Pata.Domain.Entidades.Animal.Enum;
using Pata.Domain.Entidades.Tutor;
using Pata.Domain.ObjetosValor;

namespace Pata.Domain.Tests.Entidades;

public class AnimalTests
{
    private static readonly Cpf CpfValido = new("529.982.247-25");
    private static readonly DateOnly DataAtual = new(2025, 9, 8);

    [Fact]
    public void DeveAdicionarAnimalAoTutor()
    {
        var tutor = CriarTutor();
        var nascimento = new DateOnly(2020, 9, 8);

        var animal = tutor.AdicionarAnimal("  Nina  ", Especie.Gato, nascimento, DataAtual);

        Assert.NotEqual(Guid.Empty, animal.Id);
        Assert.Equal(tutor.Id, animal.TutorId);
        Assert.Equal("Nina", animal.Nome);
        Assert.Equal(Especie.Gato, animal.Especie);
        Assert.Equal(nascimento, animal.DataNascimento);
        Assert.Equal(4, animal.CalcularIdade(new DateOnly(2025, 9, 7)));
        Assert.Contains(animal, tutor.Animais);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void DeveRejeitarNomeInvalido(string nome)
    {
        var tutor = CriarTutor();

        Assert.Throws<ArgumentException>(() =>
            tutor.AdicionarAnimal(nome, Especie.Cachorro, new DateOnly(2020, 1, 1), DataAtual));
        Assert.Empty(tutor.Animais);
    }

    [Fact]
    public void DeveRejeitarNomeNulo()
    {
        var tutor = CriarTutor();

        Assert.Throws<ArgumentNullException>(() =>
            tutor.AdicionarAnimal(null!, Especie.Cachorro, new DateOnly(2020, 1, 1), DataAtual));
        Assert.Empty(tutor.Animais);
    }

    [Fact]
    public void DeveRejeitarNomeAcimaDoLimite()
    {
        var tutor = CriarTutor();
        var nome = new string('A', Animal.TamanhoMaximoNome + 1);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            tutor.AdicionarAnimal(nome, Especie.Cachorro, new DateOnly(2020, 1, 1), DataAtual));
        Assert.Empty(tutor.Animais);
    }

    [Fact]
    public void DeveRejeitarNascimentoNoFuturo()
    {
        var tutor = CriarTutor();
        var amanha = DataAtual.AddDays(1);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            tutor.AdicionarAnimal("Nina", Especie.Gato, amanha, DataAtual));
        Assert.Empty(tutor.Animais);
    }

    [Fact]
    public void DeveRejeitarEspecieInvalida()
    {
        var tutor = CriarTutor();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            tutor.AdicionarAnimal("Nina", (Especie)999, new DateOnly(2020, 1, 1), DataAtual));
        Assert.Empty(tutor.Animais);
    }

    [Fact]
    public void DeveAlterarAnimalPeloTutor()
    {
        var tutor = CriarTutor();
        var animal = tutor.AdicionarAnimal("Nina", Especie.Gato, new DateOnly(2020, 1, 1), DataAtual);
        var novoNascimento = new DateOnly(2021, 2, 3);

        tutor.AlterarAnimal(animal.Id, "  Mel  ", Especie.Cachorro, novoNascimento, DataAtual);

        Assert.Equal("Mel", animal.Nome);
        Assert.Equal(Especie.Cachorro, animal.Especie);
        Assert.Equal(novoNascimento, animal.DataNascimento);
    }

    [Fact]
    public void DeveRemoverAnimalPeloTutor()
    {
        var tutor = CriarTutor();
        var animal = tutor.AdicionarAnimal("Nina", Especie.Gato, new DateOnly(2020, 1, 1), DataAtual);

        tutor.RemoverAnimal(animal.Id);

        Assert.Empty(tutor.Animais);
    }

    [Fact]
    public void NaoDevePermitirAlterarColecaoDiretamente()
    {
        var tutor = CriarTutor();
        var animal = tutor.AdicionarAnimal("Nina", Especie.Gato, new DateOnly(2020, 1, 1), DataAtual);
        var colecao = Assert.IsAssignableFrom<ICollection<Animal>>(tutor.Animais);

        Assert.True(colecao.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => colecao.Add(animal));
    }

    [Fact]
    public void DeveRejeitarAnimalQueNaoPertenceAoTutor()
    {
        var tutor = CriarTutor();

        Assert.Throws<KeyNotFoundException>(() =>
            tutor.RemoverAnimal(Guid.NewGuid()));
    }

    [Fact]
    public void DeveRejeitarIdentificadorVazio()
    {
        var tutor = CriarTutor();

        Assert.Throws<ArgumentException>(() => tutor.RemoverAnimal(Guid.Empty));
        Assert.Throws<ArgumentException>(() =>
            tutor.AlterarAnimal(
                Guid.Empty,
                "Nina",
                Especie.Gato,
                new DateOnly(2020, 1, 1),
                DataAtual));
    }

    [Fact]
    public void DeveRejeitarDataDeReferenciaAnteriorAoNascimento()
    {
        var tutor = CriarTutor();
        var animal = tutor.AdicionarAnimal("Nina", Especie.Gato, new DateOnly(2020, 1, 1), DataAtual);

        Assert.Throws<ArgumentException>(() =>
            animal.CalcularIdade(new DateOnly(2019, 12, 31)));
    }

    [Theory]
    [InlineData(2025, 9, 7, 4)]
    [InlineData(2025, 9, 8, 5)]
    [InlineData(2025, 9, 9, 5)]
    public void DeveCalcularIdadeAoRedorDoAniversario(
        int ano,
        int mes,
        int dia,
        int idadeEsperada)
    {
        var tutor = CriarTutor();
        var animal = tutor.AdicionarAnimal(
            "Nina",
            Especie.Gato,
            new DateOnly(2020, 9, 8),
            DataAtual);

        Assert.Equal(idadeEsperada, animal.CalcularIdade(new DateOnly(ano, mes, dia)));
    }

    [Theory]
    [InlineData(2024, 2, 28, 3)]
    [InlineData(2024, 2, 29, 4)]
    public void DeveCalcularIdadeParaNascimentoEmAnoBissexto(
        int ano,
        int mes,
        int dia,
        int idadeEsperada)
    {
        var tutor = CriarTutor();
        var animal = tutor.AdicionarAnimal(
            "Nina",
            Especie.Gato,
            new DateOnly(2020, 2, 29),
            DataAtual);

        Assert.Equal(idadeEsperada, animal.CalcularIdade(new DateOnly(ano, mes, dia)));
    }

    [Fact]
    public void NaoDeveAlterarDadosQuandoNovosDadosForemInvalidos()
    {
        var tutor = CriarTutor();
        var nascimento = new DateOnly(2020, 1, 1);
        var animal = tutor.AdicionarAnimal("Nina", Especie.Gato, nascimento, DataAtual);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            tutor.AlterarAnimal(
                animal.Id,
                "Mel",
                Especie.Cachorro,
                DataAtual.AddDays(1),
                DataAtual));

        Assert.Equal("Nina", animal.Nome);
        Assert.Equal(Especie.Gato, animal.Especie);
        Assert.Equal(nascimento, animal.DataNascimento);
    }

    private static Tutor CriarTutor() => new("Maria Silva", CpfValido, null, null);
}
