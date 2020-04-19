using System.Threading.Tasks;

public interface IRunner
{
    Task Run(string environment);
}