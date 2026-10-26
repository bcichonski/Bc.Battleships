using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bc.Battleships.Infrastructure.Providers;

public interface IPersistenceConfigurationProvider
{
    string ConnectionString { get; }
}
