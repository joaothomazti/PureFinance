﻿using Pure.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
