using Faker;
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

    for (int i = 1; i < 11; i++)
    {
        Lab1.Models.Company c1 = new Lab1.Models.Company();
        c1.Name = Faker.Company.Name();
        c1.IdCountry = db.Country.FirstOrDefault(p => p.Name == "Россия")!.IdCountry;
        db.Companies.Add(c1);

        Lab1.Models.Company c2 = new Lab1.Models.Company();
        c2.Name = Faker.Company.Name();
        c2.IdCountry = db.Country.FirstOrDefault(p => p.Name == "США")!.IdCountry;
        db.Companies.Add(c2);
    }

    db.SaveChanges();
}

using (DubininDemoContext db = new DubininDemoContext())
{
    List<Lab1.Models.Company> CompanyList = db.Companies.ToList();
    foreach (Lab1.Models.Company company in CompanyList)
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
    var users7 = from u in db.Users
                 join company in db.Companies on u.IdCompany equals company.IdCompany
                 join country in db.Country on company.IdCountry equals country.IdCountry
                 select new
                 {
                     Name = u.Name,
                     Company=company.Name,
                     Country=country.Name,
                     Age=u.Age
                 };
    foreach (var u in users7)
    {
        Console.WriteLine(u.Country + " " + u.Name + " " + u.Company+" "+u.Age);
    }

    //Группировка
    //1 способ
    var groups1 = from u in db.Users
                 group u by u.IdCompany into g
                 select new
                 {
                     g.Key,
                     Count=g.Count()
                 };
    foreach (var u in groups1)
    {
        Console.WriteLine(u.Key + " " + u.Count);
    }
    //2 способ
    var groups2 = db.Users.GroupBy(u => u.IdCompany).
        Select(g => new
        {
            g.Key,
            Count = g.Count()
        });
    foreach (var u in groups1)
    {
        Console.WriteLine(u.Key + " " + u.Count);
    }
    //Агрегатные операции
    //Наличие элементов Any()
    Console.WriteLine(db.Country.Any(u=>u.IdCountry==1));
    //Все элементы удовлетворяют критерию
    Console.WriteLine(db.Country.All(u => u.IdCountry == 1));

    //Количество элементов в выборке
    Console.WriteLine(db.Users.Count());
    //Количество пользователей старше 40
    Console.WriteLine(db.Users.Count(p=>p.Age>40));
    //Минимальное, максимальное и среднее значение
    Console.WriteLine(db.Users.Min(p=>p.Age));
    Console.WriteLine(db.Users.Max(p=>p.Age));
    Console.WriteLine(db.Users.Average(p=>p.Age));
    //Сумма
    Console.WriteLine(db.Users.Sum(p => p.Age));
    //Объединение
    var union1 = db.Users.Where(u => u.Age < 40).
        Union(db.Users.Where(u => u.Name!.Contains("Вася")));
    foreach (var u in union1)
    {
        Console.WriteLine(u.Name);
    }
    var result = db.Users.Select(p => new { Name = p.Name })
    .Union(db.Companies.Select(c => new { Name = c.Name }));
    foreach (var u in result)
    {
        Console.WriteLine(u.Name);
    }
    //Пересечение
    var intersect = db.Users.Where(u => u.Age < 40).
        Intersect(db.Users.Where(u => u.Name!.Contains("Вася")));
    foreach (var u in intersect)
    {
        Console.WriteLine(u.Name);
    }
    //Разность
    var sel1 = db.Users.Where(u => u.Age > 30);
    var sel2 = db.Users.Where(u => u.Name!.Contains("Вася"));
    var raz = sel1.Except(sel2);
    foreach (var u in raz)
    {
        Console.WriteLine(u.Name);
    }
}