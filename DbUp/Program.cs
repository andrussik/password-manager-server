using System.Reflection;
using DbUp;

var connectionString = args[0];

EnsureDatabase.For.PostgresqlDatabase(connectionString);
            
var upgrader = DeployChanges.To
    .PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .WithTransaction()
    .WithVariablesDisabled()
    .Build();

if (upgrader.IsUpgradeRequired())
{
    var scripts = upgrader.GetScriptsToExecute();
    
    Console.WriteLine("Upgrade required. Executing following scripts: ");
    
    scripts.ForEach(x => Console.WriteLine(x.Name));
    
    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
        throw result.Error;
}