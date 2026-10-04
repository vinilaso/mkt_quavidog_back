namespace Sienna.Domain.Entities.Workflow
{
    public enum TeamMemberRole
    {
        Owner,
        Administrator,
        Member
    }

    public static class TemMemberRoleExtensions
    {
        public static bool IsManager(this TeamMemberRole role)
        {
            return role is TeamMemberRole.Owner or TeamMemberRole.Administrator;
        }
    }
}
