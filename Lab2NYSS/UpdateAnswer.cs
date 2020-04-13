using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2NYSS
{
	enum UpdateStatuses
	{
		Success,
		Failed
	}
	class UpdateAnswer
	{
		public UpdateStatuses status;

		public string error;

		public List<Tuple<Threat, Threat>> changes;

		public UpdateAnswer(List<Tuple<Threat, Threat>> changes) {
			status = UpdateStatuses.Success;
			this.changes = changes;
		}

		public UpdateAnswer(string error) {
			this.error = error;
			status = UpdateStatuses.Failed;
		}
	}
}
