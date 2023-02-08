namespace Store.Domain.Commands.Interfaces;

// Command e a porta de entra como os DTOs, parte de escrita
public interface ICommand
{
    void Validate();
}