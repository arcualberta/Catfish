using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Services
{
    public class SolrService : ISolrService
    {
        private readonly string _solrCoreUrl;
        public SolrService(ICatfishAppConfiguration config)
        {
            _solrCoreUrl = config.GetSolrCoreUrl().TrimEnd('/');
        }
        public void Index(Entity entity)
        {
            //XElement xml = GetSampleDoc();
            //AddUpdateAsync(xml);

            SolrDoc doc = new SolrDoc(entity);
            AddUpdateAsync(doc);
        }

        public void Commit()
        {
            _ = CommitAsync();
        }

        public void AddUpdateAsync(SolrDoc doc)
        {
            XElement payload = new XElement("add");
            payload.Add(doc.Root);

            _ = AddUpdateAsync(payload);
        }

        public void AddUpdateAsync(List<SolrDoc> docs)
        {
            XElement payload = new XElement("add");
            foreach (var doc in docs)
                payload.Add(doc.Root);

            _ = AddUpdateAsync(payload);
        }

        public async Task AddUpdateAsync(XElement payload)
        {
            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var content = new StringContent(payload.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, "text/xml");
            using var client = new HttpClient();
            using var httpResponse = await client.PostAsync(uri, content).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task CommitAsync()
        {
            return;

            var uri = new Uri(_solrCoreUrl + "/update?commit=true");

            using var client = new HttpClient();
            using var httpResponse = await client.GetAsync(uri).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();
        }

        private XElement GetSampleDoc()
        {
            var id = Guid.NewGuid().ToString();
            return XElement.Parse(string.Format(@"
            <add><doc><field name='id'>{0}</field><field name='name_s'>change.me {1}</field></doc></add>
                ", id, DateTime.Now.ToString()));

            ////return XElement.Parse(@"<add>
            ////          <doc>
            ////            <field name='id'>P11!prod</field>
            ////            <field name='name_s'>Swingline Stapler</field>
            ////            <field name='description_t'>The Cadillac of office staplers ...</field>
            ////            <field name='skus'>
            ////              <doc>
            ////                <field name='id'>P11!S21</field>
            ////                <field name='color_s'>RED</field>
            ////                <field name='price_i'>42</field>
            ////                <field name='manuals'>
            ////                  <doc>
            ////                    <field name='id'>P11!D41</field>
            ////                    <field name='name_s'>Red Swingline Brochure</field>
            ////                    <field name='pages_i'>1</field>
            ////                    <field name='content_t'>...</field>
            ////                  </doc>
            ////                </field>
            ////              </doc>
            ////              <doc>
            ////                <field name='id'>P11!S31</field>
            ////                <field name='color_s'>BLACK</field>
            ////                <field name='price_i'>3</field>
            ////              </doc>
            ////            </field>
            ////            <field name='manuals'>
            ////              <doc>
            ////                <field name='id'>P11!D51</field>
            ////                <field name='name_s'>Quick Reference Guide</field>
            ////                <field name='pages_i'>1</field>
            ////                <field name='content_t'>How to use your stapler ...</field>
            ////              </doc>
            ////              <doc>
            ////                <field name='id'>P11!D61</field>
            ////                <field name='name_s'>Warranty Details</field>
            ////                <field name='pages_i'>42</field>
            ////                <field name='content_t'>... lifetime guarantee ...</field>
            ////              </doc>
            ////            </field>
            ////          </doc>
            ////        </add>");
        }
    }
}
