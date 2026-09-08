using Pata.Domain.Comum;
using Pata.Domain.Entidades.Animal.Enum;

namespace Pata.Domain.Entidades.Animal;

public sealed class Animal : Entidade<Guid>
{
    public const int TamanhoMaximoNome = 100;

    private Animal()
    {
    }

    internal Animal(
        Guid id,
        Guid tutorId,
        string nome,
        Especie especie,
        DateOnly dataNascimento,
        DateOnly dataAtual) : base(id)
    {
        if (tutorId == Guid.Empty)
            throw new ArgumentException("Tutor e obrigatorio.", nameof(tutorId));

        TutorId = tutorId;
        AplicarDados(nome, especie, dataNascimento, dataAtual);
    }

    public Guid TutorId { get; private set; }
    public string Nome { get; private set; } = null!;
    public Especie Especie { get; private set; }
    public DateOnly DataNascimento { get; private set; }

    public int CalcularIdade(DateOnly dataReferencia)
    {
        if (dataReferencia < DataNascimento)
            throw new ArgumentException(
                "A data de referencia nao pode ser anterior ao nascimento.",
                nameof(dataReferencia));

        var idade = dataReferencia.Year - DataNascimento.Year;

        if (DataNascimento > dataReferencia.AddYears(-idade))
            idade--;

        return idade;
    }

    internal void AlterarDados(
        string nome,
        Especie especie,
        DateOnly dataNascimento,
        DateOnly dataAtual) => AplicarDados(nome, especie, dataNascimento, dataAtual);

    private void AplicarDados(
        string nome,
        Especie especie,
        DateOnly dataNascimento,
        DateOnly dataAtual)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nome);

        var nomeNormalizado = nome.Trim();

        if (nomeNormalizado.Length > TamanhoMaximoNome)
            throw new ArgumentOutOfRangeException(
                nameof(nome),
                $"O nome deve ter no maximo {TamanhoMaximoNome} caracteres.");

        if (!System.Enum.IsDefined(especie))
            throw new ArgumentOutOfRangeException(nameof(especie), "Especie invalida.");

        if (dataNascimento > dataAtual)
            throw new ArgumentOutOfRangeException(
                nameof(dataNascimento),
                "A data de nascimento nao pode estar no futuro.");

        Nome = nomeNormalizado;
        Especie = especie;
        DataNascimento = dataNascimento;
    }
}
