namespace RotMG.Common
{
    public enum RegisterStatus
    {
        Success,
        EmailTaken,
        InvalidEmail,
        InvalidPassword,
        TooManyRegisters,
        NameTaken,
    }
    
    public enum GuildCreateStatus
    {
        Success,
        InvalidName,
        UsedName
    }

    public enum AddGuildMemberStatus
    {
        Success,
        NameNotChosen,
        AlreadyInGuild,
        InAnotherGuild,
        IsAMember,
        GuildFull,
        Error
    }
}
