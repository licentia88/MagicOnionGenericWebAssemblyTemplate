using System;
using MagicOnion;

namespace MagicT.Shared.Services;

public interface ITestService:IService<ITestService>
{
	UnaryResult<int> Sum(int a, int b);
}

