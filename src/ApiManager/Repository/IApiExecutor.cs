﻿using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Repository
{
	interface IApiExecutor
	{
		Task<string> StartAsync(TestData testData);
	}
}
