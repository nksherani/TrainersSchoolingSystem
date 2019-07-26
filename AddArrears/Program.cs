using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Utils;

namespace AddArrears
{
    class Program
    {
        static TrainersEntities db = new TrainersEntities();

        static void Main(string[] args)
        {
            bool res = ProcessClassTeachersData("arrears.xlsx");
            if (res)
                Console.WriteLine("arrears added successfully");
            else
                Console.WriteLine("something went wrong");

            Console.ReadKey();
        }
        static bool ProcessClassTeachersData(string path)
        {
            try
            {
                var package = new ExcelPackage(new FileInfo(path));
                //List<ExcelRange> list = new List<ExcelRange>();
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                var arrearsdb = db.Arrears.ToList();
                //var userid = 0;
                for (int i = workSheet.Dimension.Start.Row;
                         i <= workSheet.Dimension.End.Row;
                         i++)
                {
                    string cellValue = workSheet.Cells[i, 1].Text;
                    int temp = 0;

                    if (int.TryParse(cellValue, out temp))
                    {
                        //list.Add(workSheet.Cells[i, 1, i, workSheet.Dimension.End.Column]);
                        Arrear arr = new Arrear();
                        string grno = workSheet.Cells[i, 2].Text;
                        int studentId = db.Enrolments.Where(x => x.GRNo == grno && x.IsActive.Value).FirstOrDefault().Student.Value;
                        arr.StudentId = studentId;
                        if (arrearsdb.Where(x => x.StudentId == arr.StudentId).Count() > 0)
                        {
                            arr = arrearsdb.Where(x => x.StudentId == arr.StudentId).FirstOrDefault();
                            arr.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            arr.CreatedDate = DateTime.Now;
                        }
                        arr.Amount = Convert.ToDecimal(workSheet.Cells[i, 3].Text);
                        arr.ArrearType = "Receivable";
                        db.Arrears.AddOrUpdate(arr);
                    }
                }
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.TargetSite.Name);
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException.Source);
                    Console.WriteLine(ex.InnerException.TargetSite.Name);
                    Console.WriteLine(ex.InnerException.StackTrace);
                }
            }
            return false;
        }
    }
}
