using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2NYSS
{
	public class ThreatChanges
	{
		private int _id;
		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				StringID = $"УБИ.{value}";
			}
		}
		public string StringID
		{
			get; set;
		}
		public Dictionary<string, Tuple<string, string>> changes { get; set; }
		public ThreatChanges(int Id, Dictionary<string, Tuple<string, string>> changes)
		{
			this.Id = Id;
			this.changes = changes;
		}
	}
}
