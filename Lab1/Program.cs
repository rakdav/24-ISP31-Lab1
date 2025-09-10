using Lab1;
using Lab1.Models;

//using (DubininDemoContext db=new DubininDemoContext())
//{
//    Company c1 = new Company();
//    c1.Name = "Рога и копыта";
//    db.Companies.Add(c1);

//    Company c2 = new Company();
//    c2.Name = "Копыта и рога";
//    db.Companies.Add(c2);

//    db.SaveChanges();
//}

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
