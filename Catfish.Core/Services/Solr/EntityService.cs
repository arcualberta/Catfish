using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Solr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public class EntityService : IEntityService
	{
		protected ISolrIndexService<SolrItemModel> _solrIndexService;
		public EntityService(ISolrIndexService<SolrItemModel> srv)
		{
			_solrIndexService = srv;
		}

		public bool AddUpdateEntity(Entity entity)
		{
			bool status = true;

			foreach(MetadataSet ms in entity.MetadataSets)
			{
				foreach(BaseField field in ms.Fields)
				{
					if(typeof(TextField).IsAssignableFrom(field.GetType()))
					{
						foreach(MultilingualText val in (field as TextField).Values)
						{
							foreach (Text txt in val.Values)
							{
								SolrItemModel solrText = new SolrItemModel()
								{
									EntityGuid = entity.Id,
									MetadataSetGuid = ms.Id,
									FieldGuid = field.Id,
									//TODO: Assign ValueGuid = val.Id,
									//TODO: Assign  TextGuid = txt.Id,
									Lang = txt.Language,
									Content = txt.Value
								};

								status &= _solrIndexService.AddUpdate(solrText);
							}
						}
					}
				}
			}

			return status;
		}

	}
}
