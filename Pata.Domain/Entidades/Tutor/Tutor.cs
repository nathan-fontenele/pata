using Pata.Domain.Comum;
using Pata.Domain.Entidades.Animal.Enum;
using Pata.Domain.ObjetosValor;

namespace Pata.Domain.Entidades.Tutor;

public class Tutor : RaizAgregadaAuditavel<Guid>
{
    private readonly List<Animal.Animal> _animais = [];

    public string Nome { get; private set; }
    public Cpf Cpf { get; private set; }
    public Email? Email { get; private set; }
    public Telefone? Telefone { get; private set; }
    public IReadOnlyCollection<Animal.Animal> Animais => _animais.AsReadOnly();

    private Tutor()
    {
        Nome = null!;
        Cpf = null!;
    }

    public Tutor(string nome, Cpf cpf, Email? email, Telefone? telefone) : base(Guid.NewGuid())
    {
        ArgumentNullException.ThrowIfNull(cpf);

        Nome = ValidarNome(nome);
        Cpf = cpf;
        Email = email;
        Telefone = telefone;
    }

    public void AlterarNome(string nome) => Nome = ValidarNome(nome);

    public void AlterarEmail(Email? email) => Email = email;

    public void AlterarTelefone(Telefone? telefone) => Telefone = telefone;

    public Animal.Animal AdicionarAnimal(
        string nome,
        Especie especie,
        DateOnly dataNascimento,
        DateOnly dataAtual)
    {
        var animal = new Animal.Animal(Guid.NewGuid(), Id, nome, especie, dataNascimento, dataAtual);
        _animais.Add(animal);
        return animal;
    }

    public void AlterarAnimal(
        Guid animalId,
        string nome,
        Especie especie,
        DateOnly dataNascimento,
        DateOnly dataAtual) =>
        ObterAnimal(animalId).AlterarDados(nome, especie, dataNascimento, dataAtual);

    public void RemoverAnimal(Guid animalId)
    {
        var animal = ObterAnimal(animalId);
        _animais.Remove(animal);
    }

    private static string ValidarNome(string nome)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nome);
        return nome.Trim();
    }

    private Animal.Animal ObterAnimal(Guid animalId)
    {
        if (animalId == Guid.Empty)
            throw new ArgumentException("Animal e obrigatorio.", nameof(animalId));

        return _animais.FirstOrDefault(animal => animal.Id == animalId)
               ?? throw new KeyNotFoundException("Animal nao encontrado neste tutor.");
    }
}
