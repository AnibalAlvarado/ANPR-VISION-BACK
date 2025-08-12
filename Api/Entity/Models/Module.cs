﻿
using Entity.Models;

namespace Entity.Models
{
	public class Module : GenericModel
	{
		public string Description { get; set; }

		public virtual ICollection<FormModule> FormModules { get; set; } = new List<FormModule>();

	} 
}