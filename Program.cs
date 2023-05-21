﻿using Microsoft.Extensions.DependencyInjection;
using Time;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.CursorVisible = false;
Console.Clear();

var services = Configurator.Configure();

services.GetRequiredService<MainLoop>().Run();
