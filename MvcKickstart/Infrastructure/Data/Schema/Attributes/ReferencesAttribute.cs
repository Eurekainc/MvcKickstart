﻿using System;

namespace MvcKickstart.Infrastructure.Data.Schema.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct)]
	public class ReferencesAttribute : Attribute
	{
		public Type Type { get; set; }

		public ReferencesAttribute(Type type)
		{
			Type = type;
		}
	}
}