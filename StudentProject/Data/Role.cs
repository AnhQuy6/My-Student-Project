namespace StudentProject.Data
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual ICollection<RolePrivilege> Roleprivileges { get; set; }

        public virtual ICollection<UserRoleMapping> UserRoleMappings {  get; set; }
    }
}
