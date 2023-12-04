﻿using System.Text.RegularExpressions;

namespace Logic.Entities;

public class Email : ValueObject<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        email = (email ?? string.Empty).Trim();

        if (email.Length == 0)
            return Result.Fail<Email>("Email should not be empty");

        if(email.Length > 150)
            return Result.Fail<Email>("Email is too long");

        if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
            return Result.Fail<Email>("Email is invalid");

        return Result.Ok(new Email(email));
    }
    
    protected override bool EqualsCore(Email other)
    {
        return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }
}