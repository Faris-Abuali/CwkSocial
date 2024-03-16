using System.Runtime.Serialization;

namespace CwkSocial.Domain.Enums;


/// <summary>
/// By adding [EnumMember(Value = "Admin")] attribute to the enum values, 
/// you specify that during serialization, the enum values should be represented with 
/// the specified string values ("Admin" and "User", respectively).
/// </summary>
public enum UserRoles
{
    [EnumMember(Value = "Admin")]
    Admin,

    [EnumMember(Value = "User")]
    User
}
