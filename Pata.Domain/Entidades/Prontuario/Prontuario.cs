using Pata.Domain.Comum;

namespace Pata.Domain.Entidades.Prontuario;

public sealed class Prontuario : Entidade<Guid>
{
    private readonly List<Sintoma> _sintomas = [];

    private Prontuario()
    {
    }

    internal Prontuario(
        Guid id,
        Guid consultaId,
        IEnumerable<Sintoma> sintomas,
        string? diagnostico,
        string? prescricao,
        DateTimeOffset dataRegistro) : base(id)
    {
        if (consultaId == Guid.Empty)
            throw new ArgumentException("Consulta e obrigatoria.", nameof(consultaId));

        ArgumentNullException.ThrowIfNull(sintomas);

        var sintomasNormalizados = sintomas.ToList();

        if (sintomasNormalizados.Count == 0)
            throw new ArgumentException(
                "O prontuario deve possuir pelo menos um sintoma.",
                nameof(sintomas));

        if (sintomasNormalizados.Any(sintoma => sintoma is null))
            throw new ArgumentException("A lista contem um sintoma nulo.", nameof(sintomas));

        var possuiDuplicados = sintomasNormalizados
            .GroupBy(sintoma => sintoma.Descricao, StringComparer.OrdinalIgnoreCase)
            .Any(grupo => grupo.Count() > 1);

        if (possuiDuplicados)
            throw new ArgumentException("O prontuario contem sintomas duplicados.", nameof(sintomas));

        if (dataRegistro == default)
            throw new ArgumentException("A data de registro e obrigatoria.", nameof(dataRegistro));

        ConsultaId = consultaId;
        _sintomas.AddRange(sintomasNormalizados);
        Diagnostico = NormalizarTextoOpcional(diagnostico);
        Prescricao = NormalizarTextoOpcional(prescricao);
        DataRegistro = dataRegistro;
    }

    public Guid ConsultaId { get; private set; }
    public IReadOnlyCollection<Sintoma> Sintomas => _sintomas.AsReadOnly();
    public string? Diagnostico { get; private set; }
    public string? Prescricao { get; private set; }
    public DateTimeOffset DataRegistro { get; private set; }

    private static string? NormalizarTextoOpcional(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
