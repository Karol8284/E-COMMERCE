namespace CORE.Enums
{
    /// <summary>
    /// Role - defines user roles and permission levels in the application
    /// Used to control access to features and administrative functions
    /// </summary>
    public enum Role
    {
        /// <summary>Standard user role with limited permissions; can read books and track progress</summary>
        User,

        /// <summary>Administrator role with full system access; manages users, books, and system configuration</summary>
        Admin,
    }
}
