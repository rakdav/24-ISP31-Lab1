using Lab1;
using Lab1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using (DubininDemoContext db = new DubininDemoContext())
{
    Country russia=new Country { Name="Россия"};
    Country usa=new Country { Name="США"};
    db.Country.Add(russia);
    db.Country.Add(usa);
    db.SaveChanges();

    Company c1 = new Company();
    c1.Name = "Рога и копыта";
    c1.IdCountry = db.Country.FirstOrDefault(p=>p.Name== "Россия")!.IdCountry;
    db.Companies.Add(c1);

    Company c2 = new Company();
    c2.Name = "Копыта и рога";
    c2.IdCountry = db.Country.FirstOrDefault(p => p.Name == "США")!.IdCountry;
    db.Companies.Add(c2);

    db.SaveChanges();
}

using (DubininDemoContext db = new DubininDemoContext())
{
    List<Company> CompanyList = db.Companies.ToList();
    foreach (Company company in CompanyList)
    {
        Console.WriteLine(company.IdCompany+" "+company.Name);
    }
}
using (DubininDemoContext db = new DubininDemoContext())
{
    List<string> CompanyList = db.Companies.Select(p=>p.Name).ToList();
    foreach (string company in CompanyList)
    {
        Console.WriteLine(company);
    }
}
//Фильтрация
using (DubininDemoContext db = new DubininDemoContext())
{
    //User user1 = new User();
    //user1.Name= "Вася";
    //user1.IdCompany = 1;

    //User user2 = new User();
    //user2.Name = "Сережа";
    //user2.IdCompany = 2;
    //db.Users.Add(user1);
    //db.Users.Add(user2);
    //db.SaveChanges();

    //1 способ
    List<User> UserList = db.Users.Where(p=>p.IdCompany==1).ToList();
    foreach (User company in UserList)
    {
        Console.WriteLine(company.Name);
    }

    //2 способ
    var users=(from user in db.Users 
               where user.IdCompany==1 
               select user).ToList();
    foreach (User company in UserList)
    {
        Console.WriteLine(company.Name);
    }

    //Like
    //1 способ
    var like1=db.Users.Where(p=>EF.Functions.Like(p.Name,"_а%"));
    foreach (User l in like1)
    {
        Console.WriteLine(l.Name);
    }
    //2 способ
    var like2 = from u in db.Users 
                where EF.Functions.Like(u.Name, "_а%")
                select u;
    foreach (User l in like2)
    {
        Console.WriteLine(l.Name);
    }
    //Find/FindAsync - Для выборки одного объекта
    User? user1=db.Users.Find(2);
    Console.WriteLine(user1!.Name);

    //First/FirstOrDefault/FirstAsync/FirstOrDefaultAsync - альтернатива Find
    User? user2 = db.Users.FirstOrDefault(p=>p.IdUser==3)??new User();
    Console.WriteLine(user2!.Name);
}
//Сортировка
using (DubininDemoContext db = new DubininDemoContext())
{
    //1 способ
    var users1 = db.Users.OrderBy(p=>p.IdCompany);
    foreach (User u in users1)
    {
        Console.WriteLine(u.IdCompany+" "+u.Name);
    }
    //2 способ
    var users2=from u in db.Users
               orderby u.IdCompany
               select u;
    foreach (User u in users2)
    {
        Console.WriteLine(u.IdCompany+" "+u.Name);
    }
    //по убыванию
    var users3 = db.Users.OrderByDescending(p => p.IdCompany);
    foreach (User u in users3)
    {
        Console.WriteLine(u.IdCompany + " " + u.Name);
    }
    //сортировка по нескольким критериям
    var users4 = db.Users.OrderBy(p => p.IdCompany).ThenBy(u=>u.IdUser);
    foreach (User u in users4)
    {
        Console.WriteLine(u.IdCompany + " " + u.Name);
    }
    //соединение двух таблиц
    //1 способ
    var users5 = db.Users.Join(db.Companies,
        u => u.IdCompany,
        c => c.IdCompany,
        (u, c) => new
        {
            Name=u.Name,
            Company=c.Name
        });
    foreach (var u in users5)
    {
        Console.WriteLine(u.Name+" "+u.Company);
    }
    //2 способ
    var users6 = from u in db.Users
                 join c in db.Companies on u.IdCompany equals c.IdCompany
                 select new {Id=u.IdUser,Name=u.Name, Company=c.Name};
    foreach (var u in users6)
    {
        Console.WriteLine(u.Id+" "+u.Name + " " + u.Company);
    }

    //соединение трех таблиц

}