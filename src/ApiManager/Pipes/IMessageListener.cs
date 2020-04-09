﻿using System;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Pipes
{
	internal interface IMessageListener 
	{
		Task SubScribe(string name, Action<string> onMessage);
		Task UnSubscribeAll();
	}
}
