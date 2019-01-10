using HillPigeon.Orleans.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Runtime;
using System;

namespace HillPigeon.Orleans
{
    public class DefaultOrleansClient : IOrleansClient
    {
        private readonly IClusterClientFactory _clusterClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OrleansRoutingOptions _options;

        public DefaultOrleansClient(IClusterClientFactory clusterClientFactory,
            IHttpContextAccessor httpContextAccessor, IOptions<OrleansRoutingOptions> options)
        {
            this._clusterClientFactory = clusterClientFactory;
            this._httpContextAccessor = httpContextAccessor;
            this._options = options.Value;
        }
        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithGuidKey
        {
            var grain = this.GetClusterClient().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetRequestContext();
            return grain;
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithIntegerKey
        {
            var grain = this.GetClusterClient().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetRequestContext();
            return grain;
        }

        public TGrainInterface GetGrain<TGrainInterface>(string primaryKey, string grainClassNamePrefix = null)
            where TGrainInterface : IGrainWithStringKey
        {
            var grain = this.GetClusterClient().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetRequestContext();
            return grain;
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey
        {
            var grain = this.GetClusterClient().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            this.SetRequestContext();
            return grain;
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey
        {
            var grain = this.GetClusterClient().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            this.SetRequestContext();
            return grain;
        }

        public TGrainInterface GetGrainAutoPrimaryKey<TGrainInterface>(string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey, IGrainWithIntegerKey, IGrainWithStringKey
        {
            var type = typeof(TGrainInterface);
            if (typeof(IGrainWithIntegerCompoundKey).IsAssignableFrom(type))
            {
                int primaryKey = new Random().Next(-1000000, 0);
                return this.GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            }
            else if (typeof(IGrainWithIntegerCompoundKey).IsAssignableFrom(type))
            {
                string primaryKey = "HillPigeon#" + new Random().Next(-1000000, 0);
                return this.GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            }
            else
            {
                return this.GetGrain<TGrainInterface>(Guid.NewGuid(), grainClassNamePrefix);
            }
        }

        public TGrainInterface GetGrainAutoPrimaryKey<TGrainInterface>(string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey, IGrainWithIntegerCompoundKey
        {
            var type = typeof(TGrainInterface);
            if (typeof(IGrainWithIntegerCompoundKey).IsAssignableFrom(type))
            {
                int primaryKey = new Random().Next(-1000000, 0);
                return this.GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            }
            else
            {
                return this.GetGrain<TGrainInterface>(Guid.NewGuid(), keyExtension, grainClassNamePrefix);
            }
        }

        private IClusterClient GetClusterClient()
        {
            return this._clusterClientFactory.Create();
        }

        private void SetRequestContext()
        {
            var context = this._httpContextAccessor.HttpContext;
            HttpRequestContext requestContext = HttpRequestContexBuilder.Build(context);
            this._options?.HttpRequestContext?.Invoke(context, requestContext);
            RequestContext.Set("HttpRequest", requestContext);
        }
    }
}
