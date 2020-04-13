using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Npoi.Mapper.Attributes;

namespace Lab2NYSS
{
	public class Threat
	{
		private int _id;
		[Column("Идентификатор УБИ")]
		public int Id{ get { return _id; }
			set
			{
				_id = value;
				StringID = $"УБИ.{value}";
			}
		}

		[Ignore]
		public string StringID {
			get; set;
		}

		[Column("Наименование УБИ")]
		public string Name { get; set; }

		[Column("Описание")]
		public string Description { get; set; }

		[Column("Источник угрозы (характеристика и потенциал нарушителя)")]
		public string ThreatSource { get; set; }

		[Column("Объект воздействия")]
		public string Victim { get; set; }

		[Column("Нарушение конфиденциальности")]
		public bool Confidentiality { get; set; }

		[Column("Нарушение целостности")]
		public bool Integrity { get; set; }

		[Column("Нарушение доступности")]
		public bool Availability { get; set; }


		public Threat()
		{

		}
		public override string ToString()
		{
			return $"{StringID}: {Name} - {Id}";
		}

		public override bool Equals(object obj)
		{
			return obj is Threat threat &&
				   Id == threat.Id &&
				   Name == threat.Name &&
				   Description == threat.Description &&
				   (ThreatSource == threat.ThreatSource || (ThreatSource == null && threat.ThreatSource == "")) &&
				   Victim == threat.Victim &&
				   Confidentiality == threat.Confidentiality &&
				   Integrity == threat.Integrity &&
				   Availability == threat.Availability;
		}

		public Threat(int id, string name, string description, string threatSource, string victim, bool confidentiality, bool integrity, bool availability)
		{
			Id = id;
			StringID = $"УБИ.{id}";
			Name = name;
			Description = description;
			ThreatSource = threatSource;
			Victim = victim;
			Confidentiality = confidentiality;
			Integrity = integrity;
			Availability = availability;
		}
	}
}
