﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using ObjCRuntime;

namespace Microsoft.AppCenter.tvOS.Bindings
{
	[Native]
	public enum MSLogLevel : ulong
	{
		Verbose = 2,
		Debug = 3,
		Info = 4,
		Warning = 5,
		Error = 6,
		Assert = 7,
		None = 99
	}
}
