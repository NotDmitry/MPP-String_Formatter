using StringFormatter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringFormatter.Tests;

public class User
{
    public string FirstName { get; }
    public string LastName { get; }

    public int[] Dice { get; } = { 1, 2, 3, 4, 5 };

    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string GetGreeting()
    {
        return Core.StringFormatter.Shared.Format("Привет, {FirstName} {LastName}!", this);
    }
}
