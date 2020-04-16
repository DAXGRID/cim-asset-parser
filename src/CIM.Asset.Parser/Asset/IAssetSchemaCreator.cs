using System.Collections.Generic;
using CIM.Asset.Parser.Cim;

namespace CIM.Asset.Parser.Asset
{
    public interface IAssetSchemaCreator
    {
        void Create(IEnumerable<CimEntity> cimEntities);
    }
}
