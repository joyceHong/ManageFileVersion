﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ManageFileVersion.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TCSEntities : DbContext
    {
        public TCSEntities()
            : base("name=TCSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<alluser> alluser { get; set; }
        public virtual DbSet<asuser> asuser { get; set; }
    
        public virtual ObjectResult<Nullable<int>> IdentifyLogin3(string account, string password, string lIp)
        {
            var accountParameter = account != null ?
                new ObjectParameter("Account", account) :
                new ObjectParameter("Account", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var lIpParameter = lIp != null ?
                new ObjectParameter("lIp", lIp) :
                new ObjectParameter("lIp", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("IdentifyLogin3", accountParameter, passwordParameter, lIpParameter);
        }
    }
}