using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_Step.Interfaces
{
    public interface ITokenStorage
    {
        void SaveRefreshToken(string token);
        string? LoadRefreshToken();
        void DeleteRefreshToken();
    }
}
