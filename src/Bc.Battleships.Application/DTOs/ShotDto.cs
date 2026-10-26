namespace Bc.Battleships.Application.DTOs;

/// <summary>
/// Body of <c>POST /games/{id}/shots</c>. <paramref name="Column"/> and <paramref name="Row"/> are zero-based.
/// </summary>
public sealed record ShotDto(int Column, int Row);
