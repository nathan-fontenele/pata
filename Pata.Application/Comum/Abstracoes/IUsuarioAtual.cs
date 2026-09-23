namespace Pata.Application.Comum.Abstracoes;

public interface IUsuarioAtual
{
    string Identificador { get; }
    string Perfil { get; }
    IReadOnlyCollection<string> Papeis { get; }
}
