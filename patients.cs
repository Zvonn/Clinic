//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class patients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public patients()
        {
            this.appointments = new HashSet<appointments>();
            this.medical_records = new HashSet<medical_records>();
        }
    
        public int patient_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public System.DateTime dob { get; set; }
        public string gender { get; set; }
        public string contact_info { get; set; }
        public string address { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appointments> appointments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<medical_records> medical_records { get; set; }
    }
}
