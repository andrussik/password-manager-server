using System.Reflection;
using DbUp;

var connectionString = args[0];

EnsureDatabase.For.SqlDatabase(connectionString);
            
var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .WithTransaction()
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