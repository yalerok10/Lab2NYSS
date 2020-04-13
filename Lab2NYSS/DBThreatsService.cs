using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiteDB;

namespace Lab2NYSS
{
	class DBThreatsService
	{
		public DBThreatsService()
		{

		}

		public static List<Threat> GetThreatsPage(int PageNumber)
		{
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                // Получаем коллекцию
                var col = db.GetCollection<Threat>("threats");


				// Получаем все документы
				var result = col.Find(x => true, 15 * (PageNumber - 1), 15);
				List<Threat> ListThreats = result.ToList();
				return ListThreats;
            }
		}
		public static void SaveThreats(List<Threat> list)
		{
			using (var db = new LiteDatabase(@"MyData.db"))
			{
				var col = db.GetCollection<Threat>("threats");
				foreach (var item in list)
				{
					col.Insert(item);
				}
				col.EnsureIndex(x => x.Id);
			}
		}
		public static void ClearSavedThreats(){
			using (var db = new LiteDatabase(@"MyData.db"))
			{
				// Получаем коллекцию
				var col = db.GetCollection<Threat>("threats");
				col.DeleteAll();
			}
		}
		public static int GetPagesCount()
		{
			using (var db = new LiteDatabase(@"MyData.db"))
			{
				// Получаем коллекцию
				var col = db.GetCollection<Threat>("threats");
				return (int)Math.Ceiling(col.Count()/15.0);
			}
		}

		public static List<Threat> GetAllThreats() {
			using (var db = new LiteDatabase(@"MyData.db"))
			{
				// Получаем коллекцию
				var col = db.GetCollection<Threat>("threats");
				return col.FindAll().ToList();
			}
		}
	}
}
