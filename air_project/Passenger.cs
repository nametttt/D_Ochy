//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace air_project
{
    using System;
    using System.Collections.Generic;
    
    public partial class Passenger
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Passenger()
        {
            this.Document = new HashSet<Document>();
        }
    
        public int IdPassenger { get; set; }
        public int IdUser { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public System.DateTime Bithday { get; set; }
        public string Gender { get; set; }
        public int Citizenship { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document> Document { get; set; }
        public virtual User User { get; set; }

        public Passenger(int idUser, string surname, string name, string patronymic, DateTime bithday, string gender, int citizenship)
        {
            IdUser = idUser;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Bithday = bithday;
            Gender = gender;
            Citizenship = citizenship;
        }
    }
}
