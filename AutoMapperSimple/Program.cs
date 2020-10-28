using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperSimple
{
    class Program
    {
        // Step 0 
        // --->  Install AutoMapper From Nuget 

        // AutoMapper กับขัอมูลชนิด Collection
        // Ref : https://sysadmin.psu.ac.th/2019/07/06/automapper-กับขัอมูล-lists-และ-array/

        // รายละเอียด การ Config AutoMapper
        // Ref : https://docs.automapper.org/en/stable/Configuration.html

        static void Main(string[] args)
        {
            // ปกติ การ Mapper.Initialize จะทำครั้งเดียว เมื่อเริ่ม Start Application 
            // ข้อควรระวังในการใช้งาน :  - ถ้า Mapping Class เยอะขึ้น ขั้นตอน Initial จะช้าขึ้น กิน Ram เพิ่มขึ้น dll ใหญ่ขึ้น


            ตัวอย่างการ_MapInit_NamingConventions();
            ตัวอย่างการ_MapPath_ที่ไม่ถูกต้อง();
            ตัวอย่าง_MapObject_A_to_B();
            ตัวอย่าง_MapList_list_to_list();
            ตัวอย่าง_MapList_Group();
        }

        public static void ตัวอย่างการ_MapInit_NamingConventions()
        {
            // รายละเอียด การ Config AutoMapper (Naming-Conventions)
            // Ref :  https://docs.automapper.org/en/stable/Configuration.html#naming-conventions
            // Note : AutoMapper Create case-insensitive mappings by default

            // ตามปกติ เวลาตั้งชื่อ ตัวแปร จะตั้งตาม Naming Conventions ของภาษานั้นๆ
            // เคส string Name, string name  (จึงไม่ควรเกิดขึ้น)
            // AutoMapper จะไม่สนใจตัวเล็กตัวใหญ่ เพราะ ถือว่าเราเขียนโปรแกรมตาม Naming Convertion นั้นๆแล้ว

            Mapper.Initialize(cfg =>
            {
                cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
                cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
                cfg.CreateMap<Person, Employee1>();
            });

            var p1 = new Person()
            {
                Id = 10,
                Name = "Person 1"
            };

            var emp1 = Mapper.Map<Person, Employee1>(p1);
            Console.WriteLine("Mapp CaseSensitive -> Emp1 = " + JsonConvert.SerializeObject(emp1));
        }

        public static void ตัวอย่างการ_MapPath_ที่ไม่ถูกต้อง()
        {
            // ไม่สามารถ Map Path ObjectA ไป ObjectA ได้ 
            Mapper.Initialize(cfg => cfg.CreateMap<Person, Person>());

            // ไม่สามารถ Map Path ซ้ำกันได้
            Mapper.Initialize(cfg => cfg.CreateMap<Person, Employee1>());
            Mapper.Initialize(cfg => cfg.CreateMap<Person, Employee1>());
        }

        public static void ตัวอย่าง_MapObject_A_to_B()
        {
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Person, Employee1>()
            );

            var p1 = new Person() { Id = 1, Name = "Name1" };

            var emp = Mapper.Map<Person, Employee1>(p1);
            Console.WriteLine("Emp = " + JsonConvert.SerializeObject(emp));
        }

        public static void ตัวอย่าง_MapList_list_to_list()
        {
            // การ Mapping แบบ Tow-Way
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Person, Employee1>();
                cfg.CreateMap<Employee1, Person>();
                cfg.CreateMap<List<Person>, List<Employee1>>();
                cfg.CreateMap<List<Employee1>, List<Person>>();
            });

            var p1List = new List<Person>()
            {
                new Person{ Id = 1, Name ="Name1"  },
                new Person{ Id = 2, Name ="Name2"  },
                new Person{ Id = 3, Name ="Name3"  },
            };
            var empList = Mapper.Map<List<Person>, List<Employee1>>(p1List);
            Console.WriteLine("Emp List = " + JsonConvert.SerializeObject(empList));
        }

        public static void ตัวอย่าง_MapList_Group()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Workgroup1, Workgroup2>();
            });

            var w1 = new Workgroup1
            {
                Person = new Person { Id = 1, Name = "Name1" },
                Persons = new List<Person> { new Person { Id = 2, Name = "Name2" } }
            };

            var w2 = Mapper.Map<Workgroup1, Workgroup2>(w1);
            Console.WriteLine("Workgroup2 = " + JsonConvert.SerializeObject(w2));
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class Employee1
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
    }

    public class Workgroup1
    {
        public Person Person { get; set; }
        public List<Person> Persons { get; set; }
    }

    public class Workgroup2
    {
        public Person Person { get; set; }
        public List<Person> Persons { get; set; }

    }
}
