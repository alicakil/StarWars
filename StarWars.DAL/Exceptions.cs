﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.DAL;

public class EntityNotFoundException(string message) : Exception(message)
{
}