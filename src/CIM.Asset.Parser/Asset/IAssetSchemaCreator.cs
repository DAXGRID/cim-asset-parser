using System.Collections.Generic;
using CIM.Asset.Parser.Cim;

namespace CIM.Asset.Parser.Asset
{
    public interface IAssetSchemaCreator
    {
        Schema Create(IEnumerable<CimEntity> cimEntities);
    }
}
