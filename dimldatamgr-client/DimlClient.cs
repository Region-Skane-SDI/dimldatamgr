﻿using Newtonsoft.Json;
using RS.SDI.Diml.Core;
using RS.SDI.Diml.Core.Exceptions;
using RS.SDI.Diml.Core.GitHub;
using RS.SDI.Diml.Core.Utilities;
using RS.SDI.Diml.Data;
using System.Xml.Linq;

namespace RS.SDI.DimlDataMgr.Client
{
    public class DimlClient
    {
        public Workspace Workspace { get; set; }

        public DimlConfig Config { get; set; }

        private SortedList<string, DataProductState> DataProductStates { get; set; }

        public DimlClient()
        {
            Workspace = new Workspace();
            Config = new DimlConfig();
            Clear();
        }

        public async Task Init(string environment, string configUri)
        {
            Workspace.Environment = environment;
            Config.LoadAsync(configUri, null);
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
        public async Task PopulateSampleDataProductsAsync(string repoUri, string? xsdUri = null, string? xsdTargetNamespace = null)
        {
            // TODO: Enable validation by changing null values when DimlDataMgr is updated to use new Diml format.
            await Workspace.PopulateSampleDataProductsAsync(repoUri, null, null);
            // await Workspace.PopulateSampleDataProductsAsync(repoUri, xsdUri ?? Workspace.DefaultDataProductXsdUri, xsdTargetNamespace ?? Workspace.DefaultXsdTargetNamespace);
        }

        public async Task PopulateSampleDataSourcesAsync(string repoUri, string? xsdUri = null, string? xsdTargetNamespace = null)
        {
            // TODO: Enable validation by changing null values when DimlDataMgr is updated to use new Diml format.
            await Workspace.PopulateSampleDataSourcesAsync(repoUri, null, null);
            // await Workspace.PopulateSampleDataSourcesAsync(repoUri, xsdUri ?? Workspace.DefaultDataSourceXsdUri, xsdTargetNamespace ?? Workspace.DefaultXsdTargetNamespace);
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
                state.InformationOwner = "IT-Direktör"; // TODO: Fix this
                state.ProductOwner = "Team X"; // TODO: Fix this
                state.LastUpdate = DateTime.Now.AddHours(-1);

                DataProductStates.Add(state.Dimlid, state);
            }
        }

        public DataProductState GetDataProductState(string dimlid)
        {
            return DataProductStates.TryGetValue(dimlid, out DataProductState? value) ? value : throw new ItemMissingException(dimlid);
        }

        public DataSource GetDataSource(string dimlid)
        {
            return Workspace.GetDataSource(dimlid) ?? throw new ItemMissingException(dimlid);
        }

        public async Task<DataProductBundle> GetDataProductBundleAsync(string dimlid)
        {
            return new DataProductBundle()
            {
                Dimlid = dimlid,
                Product = Workspace.GetDataProduct(dimlid),
                State = GetDataProductState(dimlid)
            };
        }

        public async Task<DataProductBundle[]> GetAllDataProductBundlesAsync()
        {
            return Workspace.GetAllDataProducts().Select(product => new DataProductBundle()
            {
                Dimlid = product.Dimlid,
                Product = product,
                State = GetDataProductState(product.Dimlid)
            }).ToArray();
        }

        
    }
}
