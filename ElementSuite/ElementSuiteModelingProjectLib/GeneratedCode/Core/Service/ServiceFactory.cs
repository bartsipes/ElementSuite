﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Core.Service
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class ServiceFactory
	{
		public virtual ServiceFactory Instance
		{
			get;
			set;
		}

		private ServiceFactory()
		{
		}

		public virtual System.Object Resolve(System.Type serviceType)
		{
			throw new System.NotImplementedException();
		}

	}
}
