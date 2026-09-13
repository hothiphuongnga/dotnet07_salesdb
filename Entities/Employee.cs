using System;
using System.Collections.Generic;

namespace SalesDB.Entities;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Password { get; set; }
    //123456!Abc -> bam nho ra luu ma bam nho
    // USER 1:  123456!Abc -> ahjsfgusdftuasbcsdjfgidshyt
    // USER 2:  123456!Abc -> hhgysdtqkfhjdshfajsdnaksdnkas

    // khi dang nhap 
    // mk nhajp -> bawm ra  so sanh voi ma bam da luu DB


    // so sanh 2 ma baam
    




    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
