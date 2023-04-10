using Newtonsoft.Json;
using RS.SDI.Diml.Core;
using RS.SDI.Diml.Core.GitHub;
using RS.SDI.Diml.Core.Utilities;
using RS.SDI.Diml.Data;
using System.Xml.Linq;

namespace RS.SDI.DimlMgr.Client
{
    public class DimlClient
    {
        public Workspace Workspace { get; set; }

        private SortedList<string, DataProductState> DataProductStates { get; set; }

        public DimlClient(string environment)
        {
            Workspace = new Workspace();
            Workspace.Environment = environment;
            Clear();
        }

        public void Clear()
        {
            DataProductStates = new SortedList<string, DataProductState>();
        }

        public DataProductState[] GetAllDataProductStates() => DataProductStates.Values.ToArray();

        /// <summary>
        /// This method will be used to populate the data catalog for our demo
        /// </summary>
        /// <param name="repoUri"></param>
        /// <returns></returns>
        public async Task PopulateSampleDataProductsAsync(string repoUri, string xsdUri, string xsdTargetNamespace)
        {
            await Workspace.PopulateSampleDataProductsAsync(repoUri, xsdUri, xsdTargetNamespace);
        }

        public async Task PopulateSampleStatesAsync()
        { 
            foreach (var dataProduct in Workspace.GetAllDataProducts())
            {
                var state = new DataProductState();
                state.Dimlid = dataProduct.Dimlid;
                var dimlidHashCode = dataProduct.Dimlid.GetHashCode();
                state.ViewCount = Math.Abs(dimlidHashCode % 1000);
                state.LikeCount = Math.Abs(dimlidHashCode % 100);
                state.IsAvailable = true;
                state.InformationOwner = "N N"; // TODO: Fix this
                state.ProductOwner = "N N"; // TODO: Fix this
                state.LastUpdate = DateTime.Now.AddHours(-1);

                DataProductStates.Add(state.Dimlid, state);
            }
        }

        public DataProductState? GetDataProductState(string dimlid)
        {
            if (DataProductStates.ContainsKey(dimlid))
                return DataProductStates[dimlid];
            else
                return null;
        }

        public async Task<DataProductBundle> GetDataProductBundleAsync(string dimlid) => new DataProductBundle { Dimlid = dimlid, Product = Workspace.GetDataProduct(dimlid), State = GetDataProductState(dimlid) };

        public async Task<DataProductBundle[]> GetAllDataProductBundlesAsync()
        {
            var bundles = Workspace.GetAllDataProducts().Select(product => new DataProductBundle { Dimlid = product.Dimlid, Product = product, State = GetDataProductState(product.Dimlid) }).ToArray();
            return bundles;
        }
    }
}
