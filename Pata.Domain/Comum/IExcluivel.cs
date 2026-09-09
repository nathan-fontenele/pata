namespace Pata.Domain.Comum;

public interface IExcluivel
{
    bool Excluido { get; }
    DateTime? ExcluidoEm { get; }
    string? ExcluidoPor { get; }

    void Excluir(DateTime excluidoEm, string excluidoPor);

    void Recuperar();
}
