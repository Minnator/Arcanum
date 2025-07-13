using Nexus.Core;

namespace Arcanum.Core;

public partial class User(string name, int age) : INexus
{
   [IgnoreModifiable]
   public string Name = name;
   public int Age = age;
}

[ExplicitProperties]
public partial class Company(string name, int numberEmployees) : INexus
{
   [AddModifiable]
   public string Name = name;
   public int NumberEmployees = numberEmployees;
}