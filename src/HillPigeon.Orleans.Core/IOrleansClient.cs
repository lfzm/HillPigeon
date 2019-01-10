using Orleans;
using System;

namespace HillPigeon.Orleans
{
    public interface IOrleansClient
    {
        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithGuidKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithIntegerKey;

        TGrainInterface GetGrain<TGrainInterface>(string primaryKey, string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithStringKey;

        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithGuidCompoundKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithIntegerCompoundKey;


        TGrainInterface GetGrainAutoPrimaryKey<TGrainInterface>(string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithGuidKey, IGrainWithIntegerKey, IGrainWithStringKey;

        TGrainInterface GetGrainAutoPrimaryKey<TGrainInterface>(string keyExtension, string grainClassNamePrefix = null)
           where TGrainInterface : IGrainWithGuidCompoundKey, IGrainWithIntegerCompoundKey;
    }
}
