﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Clinic
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_clinicEntities : DbContext
    {
        public db_clinicEntities()
            : base("name=db_clinicEntities")
        {
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        private static db_clinicEntities _context;
        public static db_clinicEntities GetContext()
        {
            if (_context == null)
                _context = new db_clinicEntities();
            return _context;
        }
        public virtual DbSet<appointments> appointments { get; set; }
        public virtual DbSet<doctors> doctors { get; set; }
        public virtual DbSet<drugs> drugs { get; set; }
        public virtual DbSet<medical_records> medical_records { get; set; }
        public virtual DbSet<patients> patients { get; set; }
        public virtual DbSet<prescriptions> prescriptions { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<speciallizations> speciallizations { get; set; }
        public virtual DbSet<statuses> statuses { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
